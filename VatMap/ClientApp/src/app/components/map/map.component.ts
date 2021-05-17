import { Component, Inject, OnInit } from '@angular/core';

import { Plane } from '../../model/plane';
import { Map, Overlay, View } from "ol";
import { Layer, Tile, Vector } from 'ol/layer';
import { Vector as VectorSource } from 'ol/source';
import { OSM } from 'ol/source';
import { fromLonLat } from 'ol/proj';
import { Feature } from 'ol';
import { Point } from 'ol/geom';
import { Style, Icon, Text } from 'ol/style';

import { HttpService } from 'src/app/services/http.service';
import { Snapshot } from 'src/app/model/snapshot';
import { SnapshotService } from 'src/app/services/snapshot.service';
import { LogProvider } from 'src/app/providers/log.provider';
import OverlayPositioning from 'ol/OverlayPositioning';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {

  public map: any;
  public vm: Snapshot | undefined;
  private vectorLayer = new Vector({});
  private log: LogProvider = new LogProvider("MapComponent");
  private popup: any;

  constructor(private snapshotService: SnapshotService, @Inject(DOCUMENT) document: typeof DOCUMENT) { }

  private initMap(): void {
    this.log.log("preparing map");
    var osmLayer = new Tile({
      source: new OSM(),
      opacity: 1
    });

    this.log.log("drawing map");
    this.map = new Map({
      target: 'map',
      layers: [
        osmLayer,
        this.vectorLayer
      ],
      view: new View({
        center: fromLonLat([17, 50]),
        zoom: 4
      })
    });
  }

  private initOverlay(): void {
    const popupElement: any = document.getElementById("popupDiv");
    this.popup = new Overlay({
      element: popupElement,
      positioning: OverlayPositioning.BOTTOM_CENTER,
      stopEvent: false,
      offset: [32, 0]
    });
    this.map.addOverlay(this.popup);


    const me = this;
    this.map.on('pointermove', function (evt: any) {
      var feature = me.map.forEachFeatureAtPixel(evt.pixel, function (feature: Feature) {
        me.log.log("Feature clicked. " + JSON.stringify(feature.get("name")));
        return feature;
      });
      const elm: any = document.getElementById("popupDiv");
      if (feature) {
        var coordinates = feature.getGeometry().getCoordinates();
        me.log.log("A " + feature.get('name'));
        me.log.log("B " + elm.innerHTML);
        if (feature.get('name') === elm.innerHTML) {
          if (elm.style.display == "none")
            elm.style.display = "block";
          else
            elm.style.display = "none";
        } else {
          const plane: Plane | undefined = me.vm?.planes.find(q => q.callsign == feature.get('name'));
          me.popup.setPosition(coordinates);
          elm.style.display = "block";
          if (plane === undefined)
            elm.innerHTML = "???";
          else
            elm.innerHTML = "<b>" + plane.callsign + "</b><br />" + plane.altitude + "ft";
        }
      } else {
        elm.style.display = "none";
      }
    });
  }

  ngOnInit(): void {

    this.initMap();
    this.initOverlay();
    this.initData();
  }

  private initData(): void {
    this.log.log("downloading data");
    this.snapshotService.get().subscribe(
      ret => this.updateVectorLayer(ret),
      err => {
        this.log.log("error!");
        console.log(err);
      }
    );
  }

  private degToRad(val: number): number {
    return val / 180.0 * Math.PI;
  }

  private updateVectorLayer(ret: Snapshot): void {
    {
      this.log.log("data downloaded");
      this.log.log(JSON.stringify(ret));
      this.vm = ret;
      const features: Feature[] = [];
      for (let plane of this.vm.planes) {
        const iconFeature: Feature = new Feature({
          geometry: new Point(fromLonLat([plane.gpsHistory[0].longitude, plane.gpsHistory[0].latitude])),
          name: plane.callsign
        });
        const iconStyle = new Style({
          image: new Icon({
            src: 'https://icons.iconarchive.com/icons/unclebob/spanish-travel/24/plane-icon.png',
            rotation: this.degToRad(plane.heading),
            size: [32, 32]
          }),
          text: new Text({
            text: plane.callsign,
            offsetY: 20,
            font: '12px sans-serif'
          })
        });
        iconFeature.setStyle(iconStyle);
        features.push(iconFeature);
      }
      this.log.log("Total features: " + features.length);
      const vectorSource = new VectorSource({
        features: features
      });
      this.vectorLayer.setSource(vectorSource);
      this.log.log("map updated");
    }
  }

}
