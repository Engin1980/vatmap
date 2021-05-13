import { Component, OnInit } from '@angular/core';

declare var ol: any;

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {

  public map: any;

  constructor() { }

  ngOnInit(): void {

    var osmLayer = new ol.layer.Tile({
      source: new ol.source.OSM(),
      opacity: 1
    });

    this.map = new ol.Map({
      target: 'map',
      layers: [
        osmLayer
      ],
      view: new ol.View({
        center: ol.proj.fromLonLat([50,17]),
        zoom: 8
      })
    });
  }

}
