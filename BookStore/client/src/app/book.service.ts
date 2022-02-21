import { Injectable } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { BookDetail } from './models/BookDetail';

import { BookItem } from './models/BookItem';
import { GraphQLResponse } from './utils/GraphQLResponse';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  constructor(private apollo: Apollo) { }

  getAllBookItems$(): Observable<BookItem[]> {
    return this.apollo.watchQuery<GraphQLResponse<'books', BookItem[]>>({
      query: gql`
      {
        books {
          id,
          title,
          publisher,
          authors {
            name
          },
          averageReview
        }
      }`
    }).valueChanges.pipe(map(x => x.data.books));
  }

  getBookDetail$(id: string): Observable<BookDetail> {
    return this.apollo.watchQuery<GraphQLResponse<'book', BookDetail>>({
      query: gql`
      {
        book(id: "${id}") {
          id,
          title,
          description,
          imageUrl,
          publisher,
          publishedDate,          
          length,
          authors {
            id,
            name
          },
          reviews {
            id,
            rating,
            title,
            description
          }
          averageReview
        }
      }`
    }).valueChanges.pipe(map(x => x.data.book));
  }
}
