import { ExponentItemVM } from './ExponentItemVM';
import { TopicVM } from './TopicVM';
import { UserVM } from './UserVM';

export interface ExhibitionVM {
  id: number;
  title: string;
  description: string;
  date: Date;
  expired: boolean;
  started: boolean;
  organizer: UserVM;
  cover: string;
  subscribers: number;
  items: ExponentItemVM[];
  topics: TopicVM[];
  averagePrice: number;
}
