import { TopicVM } from './TopicVM';

export interface FilterVM {
  creatorName: string;
  dateFrom: Date;
  dateTo: Date;
  avgPriceMin: number;
  avgPriceMax: number;
  description: string;
  topics: string[];
}
