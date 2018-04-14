import { CategoryViewModel } from './categoryviewmodel';
import { TagViewModel } from './tagviewmodel';
export interface Postviewmodel {
    id: number;
    title: string;
    content: string;
    shortContent: string;
    category: CategoryViewModel;
    dateCreated: string;
    tags: TagViewModel[];
}
