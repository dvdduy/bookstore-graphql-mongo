import { Component } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { BookService } from './book.service';
import { BookDetail } from './models/BookDetail';
import { BookItem } from './models/BookItem';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  title = 'client';

  // all books in type of observable
  books$: Observable<BookItem[]>;

  // keep current book opened in detail mode
  bookDetail$: Observable<BookDetail | undefined> | undefined;

  // keep current selected book
  selectedBook: BookItem | undefined;
  
  // Loading and error states
  isLoadingDetail = false;
  detailErrorMessage: string | undefined;
  
  constructor(public bookService: BookService) {      
    this.books$ = bookService.getAllBookItems$().pipe(
      catchError(error => {
        console.error('Failed to load books:', error);
        return of([]); // Return empty array on error
      })
    );    
  }

  onSelectBook(selectedBook: BookItem): void {
    this.selectedBook = selectedBook;
    this.isLoadingDetail = true;
    this.detailErrorMessage = undefined;
    
    // Use async pipe in template to handle subscription automatically
    this.bookDetail$ = this.bookService.getBookDetail$(selectedBook.id).pipe(
      tap(() => {
        this.isLoadingDetail = false;
      }),
      catchError(error => {
        console.error('Failed to load book details:', error);
        this.isLoadingDetail = false;
        this.detailErrorMessage = 'Failed to load book details. Please try again.';
        return of(undefined);
      })
    );
  }
}
