import { ExponentItemVM } from './ExponentItemVM';
import { UserVM } from './UserVM';

export interface ExhibitionVM {
  id: number;
  title: string;
  description: string;
  date: Date;
  organizer: UserVM;
  cover: string;
  items: ExponentItemVM[];
  averagePrice: number;
}
