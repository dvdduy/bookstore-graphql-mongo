import { Component, Input } from '@angular/core';
import { BookDetail } from '../models/BookDetail';

@Component({
  selector: 'app-book-detail',
  templateUrl: './book-detail.component.html'
})
export class BookDetailComponent {
  @Input() book!: BookDetail;
}
