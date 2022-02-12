import { UserVM } from './UserVM';

export interface ExhibitionVM {
  title: string;
  description: string;
  startingDate: Date;
  organizer: UserVM;
}
