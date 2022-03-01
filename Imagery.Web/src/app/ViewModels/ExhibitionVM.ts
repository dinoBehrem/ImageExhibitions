import { ExponentItemVM } from './ExponentItemVM';
import { UserVM } from './UserVM';

export interface ExhibitionVM {
  id: number;
  title: string;
  description: string;
  startingDate: Date;
  organizer: UserVM;
  cover: string;
  items: ExponentItemVM[];
}
