import { Author } from "./Author";
import { Review } from "./Review";

export class BookDetail {
    id!: string;
    title!: string;
    description!: string;
    imageUrl!: string;
    publishedDate!: Date;
    publisher!: string;
    averageReview: number | undefined;    
    length!: number;
    authors: Author[] = [];
    reviews: Review[]= [];
}