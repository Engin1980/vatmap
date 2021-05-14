import { Gps } from './gps';

export class Plane {
  public callsign: string | null = null;
  public gps: Gps = new Gps();
  public altitude: number = 0;
  public heading: number = 0;
}
