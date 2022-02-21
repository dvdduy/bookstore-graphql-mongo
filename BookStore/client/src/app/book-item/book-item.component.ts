import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BookItem } from '../models/BookItem';

@Component({
  selector: 'app-book-item',
  templateUrl: './book-item.component.html'
})
export class BookItemComponent  {

  // indicate whether the current item is selected and active
  @Input() isActive = false;

  // book of the current item
  @Input() book!: BookItem;

  // event when this item is selected
  @Output() selected: EventEmitter<BookItem> = new EventEmitter();

  onClick(): void {    
    this.selected.emit(this.book);
  }
}
