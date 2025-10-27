import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BookItemComponent } from './book-item.component';
import { BookItem } from '../models/BookItem';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('BookItemComponent', () => {
  let component: BookItemComponent;
  let fixture: ComponentFixture<BookItemComponent>;

  const mockBook: BookItem = {
    id: '123',
    title: 'Test Book',
    publishedDate: new Date('2023-01-01'),
    publisher: 'Test Publisher',
    authors: [
      { id: '1', name: 'Author 1' },
      { id: '2', name: 'Author 2' }
    ],
    averageReview: 4.5
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookItemComponent],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookItemComponent);
    component = fixture.componentInstance;
    component.book = mockBook; // Set required input
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Input Properties', () => {
    it('should accept book input', () => {
      component.book = mockBook;
      fixture.detectChanges();

      expect(component.book).toEqual(mockBook);
      expect(component.book.title).toBe('Test Book');
    });

    it('should accept isActive input and default to false', () => {
      expect(component.isActive).toBe(false);

      component.isActive = true;
      fixture.detectChanges();

      expect(component.isActive).toBe(true);
    });

    it('should handle book with no reviews', () => {
      const bookWithNoReviews: BookItem = {
        ...mockBook,
        averageReview: undefined
      };
      component.book = bookWithNoReviews;
      fixture.detectChanges();

      expect(component.book.averageReview).toBeUndefined();
    });

    it('should handle book with single author', () => {
      const bookWithSingleAuthor: BookItem = {
        ...mockBook,
        authors: [{ id: '1', name: 'Single Author' }]
      };
      component.book = bookWithSingleAuthor;
      fixture.detectChanges();

      expect(component.book.authors.length).toBe(1);
      expect(component.book.authors[0].name).toBe('Single Author');
    });
  });

  describe('Click Event', () => {
    it('should emit selected event when onClick is called', (done) => {
      component.book = mockBook;

      component.selected.subscribe((book: BookItem) => {
        expect(book).toBe(mockBook);
        expect(book.id).toBe('123');
        expect(book.title).toBe('Test Book');
        done();
      });

      component.onClick();
    });

    it('should emit the correct book when clicked multiple times', () => {
      let emittedBook: BookItem | undefined;
      
      component.selected.subscribe((book: BookItem) => {
        emittedBook = book;
      });

      component.onClick();
      expect(emittedBook).toBe(mockBook);

      component.onClick();
      expect(emittedBook).toBe(mockBook);
    });

    it('should emit event regardless of isActive state', () => {
      let emitCount = 0;

      component.selected.subscribe(() => {
        emitCount++;
      });

      // Click when not active
      component.isActive = false;
      component.onClick();
      expect(emitCount).toBe(1);

      // Click when active
      component.isActive = true;
      component.onClick();
      expect(emitCount).toBe(2);
    });

    it('should emit different books when book input changes', () => {
      const emittedBooks: BookItem[] = [];

      component.selected.subscribe((book: BookItem) => {
        emittedBooks.push(book);
      });

      // Click with first book
      component.book = mockBook;
      component.onClick();

      // Change book and click
      const newBook: BookItem = {
        id: '456',
        title: 'New Book',
        publishedDate: new Date('2023-06-01'),
        publisher: 'New Publisher',
        authors: [{ id: '3', name: 'New Author' }],
        averageReview: 3.5
      };
      component.book = newBook;
      component.onClick();

      expect(emittedBooks.length).toBe(2);
      expect(emittedBooks[0]).toBe(mockBook);
      expect(emittedBooks[1]).toBe(newBook);
    });
  });

  describe('Component State', () => {
    it('should toggle isActive state correctly', () => {
      component.isActive = false;
      expect(component.isActive).toBe(false);

      component.isActive = true;
      expect(component.isActive).toBe(true);

      component.isActive = false;
      expect(component.isActive).toBe(false);
    });

    it('should maintain book data when isActive changes', () => {
      component.book = mockBook;
      component.isActive = false;
      fixture.detectChanges();

      expect(component.book).toBe(mockBook);

      component.isActive = true;
      fixture.detectChanges();

      expect(component.book).toBe(mockBook);
    });
  });
});
