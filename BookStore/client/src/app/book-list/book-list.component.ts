import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BookItem } from '../models/BookItem';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html'
})
export class BookListComponent {

  // all books
  @Input() books: BookItem[] | null = [];

  // the book being selected
  @Input() selectedBook: BookItem | undefined;

  // event when a book is selected
  @Output() selected: EventEmitter<BookItem> = new EventEmitter();

  onBookSelected(selectedBook: BookItem): void {
    this.selected.emit(selectedBook);
  }
}
