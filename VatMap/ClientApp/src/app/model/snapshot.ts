import { Plane } from './plane';
import { Atc } from './atc';

export class Snapshot {
  public planes: Plane[] = [];
  public atcs: Atc[] = [];
  public date: Date | null = null;
}
