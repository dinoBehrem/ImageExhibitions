import { TopicVM } from './TopicVM';

export interface MyExhibitionVM {
  id: number;
  title: string;
  description: string;
  date: Date;
  expired: boolean;
  started: boolean;
  cover: string;
  subscribers: number;
  items: number;
  soldItems: number;
  profit: number;
  topics: TopicVM[];
}
