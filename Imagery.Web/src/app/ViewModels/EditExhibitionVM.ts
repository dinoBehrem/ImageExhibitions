import { TopicVM } from './TopicVM';

export interface EditExhibitionVM {
  id: number;
  title: string;
  description: string;
  date: Date;
  topics: TopicVM[];
}
