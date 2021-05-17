import { Gps } from './gps';

export class Plane {
  public callsign: string | undefined;
  public gpsHistory : Gps[] = [];
  public altitude: number = 0;
  public heading: number = 0;
}
