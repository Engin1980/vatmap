import { Component, OnInit } from '@angular/core';

import 'ol/ol.css';
import { HttpService } from 'src/app/services/http.service';
import { Snapshot } from 'src/app/model/snapshot';
import { SnapshotService } from 'src/app/services/snapshot.service';
declare var ol: any;

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {

  public map: any;
  public vm: Snapshot | null = null;
  public vectorLayer = new ol.layer.Vector({});

  constructor(private snapshotService: SnapshotService) { }

  ngOnInit(): void {

    var osmLayer = new ol.layer.Tile({
      source: new ol.source.OSM(),
      opacity: 1
    });

    this.map = new ol.Map({
      target: 'map',
      layers: [
        osmLayer,
        this.vectorLayer
      ],
      view: new ol.View({
        center: ol.proj.fromLonLat([17, 50]),
        zoom: 4
      })
    });

    this.snapshotService.get().subscribe(
      ret => {
        this.vm = ret;
        const features: ol.Feature[] = [];
        for (let plane of this.vm.planes) {
          const iconFeature = new ol.Feature({
            geometry: new ol.geom.Point(ol.proj.fromLonLat([plane.gps.longitude, plane.gps.latitude])),
            name: plane.callsign
          });
          const iconStyle = new ol.style.Style({
            image: new ol.style.Icon({
              src: 'https://icons.iconarchive.com/icons/unclebob/spanish-travel/24/plane-icon.png',
              rotation: this.degToRad(plane.heading),
              size: [32, 32]
            }),
            text: new ol.style.Text({
              text: plane.callsign,
              offsetY: 20,
              font: '12px sans-serif'
            })
          });
          iconFeature.setStyle(iconStyle);
          features.push(iconFeature);
        }
        const vectorSource = new ol.source.Vector({
          features: features
        });
        this.vectorLayer.setSource(vectorSource);
      },
      err => console.log(err)
    );
  }

  private degToRad(val: number): number {
    return val / 180.0 * Math.PI;
  }

}
