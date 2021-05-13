import { Gps } from './gps';

export class Plane {
  public callsign: string|null = null;
  public gps: Gps = new Gps(0, 0);
  public altitude: number = 0;
}
