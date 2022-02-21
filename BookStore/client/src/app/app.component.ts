import { Component } from '@angular/core';
import { Observable } from 'rxjs';
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
  bookDetail: BookDetail | undefined;

  // keep current selected book
  selectedBook: BookItem | undefined;
  
  constructor(public bookService: BookService) {      
    this.books$ = bookService.getAllBookItems$();    
  }

  onSelectBook(selectedBook: BookItem): void {
    this.selectedBook = selectedBook;

    this.bookService.getBookDetail$(selectedBook.id)
      .subscribe(x => { this.bookDetail = x;});
  }
}
