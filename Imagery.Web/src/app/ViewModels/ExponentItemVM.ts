import { DimensionsVM } from './DimensionsVM';

export interface ExponentItemVM {
  id: number;
  name: string;
  creator: string;
  description: string;
  dimensions: DimensionsVM[];
  image: string;
}
