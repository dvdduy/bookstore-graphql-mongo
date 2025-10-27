import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BookListComponent } from './book-list.component';
import { BookItem } from '../models/BookItem';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('BookListComponent', () => {
  let component: BookListComponent;
  let fixture: ComponentFixture<BookListComponent>;

  const mockBooks: BookItem[] = [
    {
      id: '1',
      title: 'Test Book 1',
      publishedDate: new Date('2023-01-01'),
      publisher: 'Publisher 1',
      authors: [{ id: '1', name: 'Author 1' }],
      averageReview: 4.5
    },
    {
      id: '2',
      title: 'Test Book 2',
      publishedDate: new Date('2023-02-01'),
      publisher: 'Publisher 2',
      authors: [{ id: '2', name: 'Author 2' }],
      averageReview: 4.0
    },
    {
      id: '3',
      title: 'Test Book 3',
      publishedDate: new Date('2023-03-01'),
      publisher: 'Publisher 3',
      authors: [{ id: '3', name: 'Author 3' }],
      averageReview: 3.5
    }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookListComponent],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookListComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Input Properties', () => {
    it('should accept books input', () => {
      component.books = mockBooks;
      fixture.detectChanges();

      expect(component.books).toEqual(mockBooks);
      expect(component.books?.length).toBe(3);
    });

    it('should handle null books input', () => {
      component.books = null;
      fixture.detectChanges();

      expect(component.books).toBeNull();
    });

    it('should handle empty books array', () => {
      component.books = [];
      fixture.detectChanges();

      expect(component.books).toEqual([]);
      expect(component.books.length).toBe(0);
    });

    it('should accept selected book input', () => {
      const selectedBook = mockBooks[0];
      component.selectedBook = selectedBook;
      fixture.detectChanges();

      expect(component.selectedBook).toBe(selectedBook);
    });

    it('should handle undefined selected book', () => {
      component.selectedBook = undefined;
      fixture.detectChanges();

      expect(component.selectedBook).toBeUndefined();
    });
  });

  describe('Output Events', () => {
    it('should emit selected event when onBookSelected is called', (done) => {
      const selectedBook = mockBooks[1];

      component.selected.subscribe((book: BookItem) => {
        expect(book).toBe(selectedBook);
        expect(book.id).toBe('2');
        expect(book.title).toBe('Test Book 2');
        done();
      });

      component.onBookSelected(selectedBook);
    });

    it('should emit correct book when different books are selected', () => {
      let emittedBook: BookItem | undefined;
      
      component.selected.subscribe((book: BookItem) => {
        emittedBook = book;
      });

      // Select first book
      component.onBookSelected(mockBooks[0]);
      expect(emittedBook).toBe(mockBooks[0]);

      // Select second book
      component.onBookSelected(mockBooks[1]);
      expect(emittedBook).toBe(mockBooks[1]);

      // Select third book
      component.onBookSelected(mockBooks[2]);
      expect(emittedBook).toBe(mockBooks[2]);
    });

    it('should emit event multiple times for same book', () => {
      let emitCount = 0;

      component.selected.subscribe(() => {
        emitCount++;
      });

      component.onBookSelected(mockBooks[0]);
      component.onBookSelected(mockBooks[0]);
      component.onBookSelected(mockBooks[0]);

      expect(emitCount).toBe(3);
    });
  });

  describe('Component Behavior', () => {
    it('should initialize with default values', () => {
      expect(component.books).toEqual([]);
      expect(component.selectedBook).toBeUndefined();
    });

    it('should maintain books list after selection', () => {
      component.books = mockBooks;
      component.onBookSelected(mockBooks[0]);

      expect(component.books).toEqual(mockBooks);
    });

    it('should update selectedBook when new selection is made', () => {
      component.books = mockBooks;
      component.selectedBook = mockBooks[0];
      fixture.detectChanges();

      expect(component.selectedBook).toBe(mockBooks[0]);

      component.selectedBook = mockBooks[1];
      fixture.detectChanges();

      expect(component.selectedBook).toBe(mockBooks[1]);
    });
  });
});
