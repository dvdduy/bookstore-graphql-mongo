import { Author } from "./Author";

export class BookItem {
    id!: string;
    title!: string;
    publishedDate!: Date;
    publisher!: string;
    averageReview: number | undefined;
    authors: Author[] = [];
}