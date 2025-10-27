import { TestBed, ComponentFixture } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { BookService } from './book.service';
import { of, throwError } from 'rxjs';
import { BookItem } from './models/BookItem';
import { BookDetail } from './models/BookDetail';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let mockBookService: jasmine.SpyObj<BookService>;

  const mockBookItems: BookItem[] = [
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
    }
  ];

  const mockBookDetail: BookDetail = {
    id: '1',
    title: 'Test Book 1',
    description: 'A test description',
    imageUrl: 'http://example.com/image.jpg',
    publisher: 'Publisher 1',
    publishedDate: new Date('2023-01-01'),
    length: 300,
    authors: [{ id: '1', name: 'Author 1' }],
    reviews: [
      { id: '1', rating: 5, title: 'Great', description: 'Amazing' }
    ],
    averageReview: 5.0
  };

  beforeEach(async () => {
    mockBookService = jasmine.createSpyObj('BookService', [
      'getAllBookItems$',
      'getBookDetail$'
    ]);

    mockBookService.getAllBookItems$.and.returnValue(of(mockBookItems));

    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      providers: [
        { provide: BookService, useValue: mockBookService }
      ],
      schemas: [NO_ERRORS_SCHEMA] // Ignore child components for unit test
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should have title "client"', () => {
    expect(component.title).toBe('client');
  });

  describe('Initialization', () => {
    it('should load books on init', (done) => {
      fixture.detectChanges(); // Triggers constructor

      component.books$.subscribe(books => {
        expect(books).toEqual(mockBookItems);
        expect(books.length).toBe(2);
        expect(mockBookService.getAllBookItems$).toHaveBeenCalled();
        done();
      });
    });

    it('should handle error when loading books fails', (done) => {
      mockBookService.getAllBookItems$.and.returnValue(
        throwError(() => new Error('Network error'))
      );
      
      // Create a new component instance after changing the mock
      const errorFixture = TestBed.createComponent(AppComponent);
      const errorComponent = errorFixture.componentInstance;
      errorFixture.detectChanges();

      errorComponent.books$.subscribe(books => {
        expect(books).toEqual([]); // Should return empty array on error
        done();
      });
    });

    it('should initialize with no selected book', () => {
      fixture.detectChanges();
      expect(component.selectedBook).toBeUndefined();
      expect(component.bookDetail$).toBeUndefined();
    });

    it('should initialize with loading state as false', () => {
      fixture.detectChanges();
      expect(component.isLoadingDetail).toBe(false);
    });

    it('should initialize with no error message', () => {
      fixture.detectChanges();
      expect(component.detailErrorMessage).toBeUndefined();
    });
  });

  describe('onSelectBook', () => {
    beforeEach(() => {
      fixture.detectChanges(); // Initialize component
    });

    it('should set selected book', () => {
      const selectedBook = mockBookItems[0];
      mockBookService.getBookDetail$.and.returnValue(of(mockBookDetail));

      component.onSelectBook(selectedBook);

      expect(component.selectedBook).toBe(selectedBook);
    });

    it('should load book details when book is selected', (done) => {
      const selectedBook = mockBookItems[0];
      mockBookService.getBookDetail$.and.returnValue(of(mockBookDetail));

      component.onSelectBook(selectedBook);

      expect(mockBookService.getBookDetail$).toHaveBeenCalledWith(selectedBook.id);
      
      component.bookDetail$?.subscribe(detail => {
        expect(detail).toEqual(mockBookDetail);
        done();
      });
    });

    it('should set loading state to true when selecting book', () => {
      const selectedBook = mockBookItems[0];
      mockBookService.getBookDetail$.and.returnValue(of(mockBookDetail));

      component.onSelectBook(selectedBook);

      expect(component.isLoadingDetail).toBe(true);
    });

    it('should clear error message when selecting new book', () => {
      component.detailErrorMessage = 'Previous error';
      const selectedBook = mockBookItems[0];
      mockBookService.getBookDetail$.and.returnValue(of(mockBookDetail));

      component.onSelectBook(selectedBook);

      expect(component.detailErrorMessage).toBeUndefined();
    });

    it('should set loading to false after book details load successfully', (done) => {
      const selectedBook = mockBookItems[0];
      mockBookService.getBookDetail$.and.returnValue(of(mockBookDetail));

      component.onSelectBook(selectedBook);

      component.bookDetail$?.subscribe(() => {
        expect(component.isLoadingDetail).toBe(false);
        done();
      });
    });

    it('should handle error when loading book details fails', (done) => {
      const selectedBook = mockBookItems[0];
      mockBookService.getBookDetail$.and.returnValue(
        throwError(() => new Error('Failed to load details'))
      );

      component.onSelectBook(selectedBook);

      component.bookDetail$?.subscribe(detail => {
        expect(detail).toBeUndefined();
        expect(component.isLoadingDetail).toBe(false);
        expect(component.detailErrorMessage).toBe('Failed to load book details. Please try again.');
        done();
      });
    });

    it('should allow selecting different books consecutively', (done) => {
      mockBookService.getBookDetail$.and.returnValue(of(mockBookDetail));

      // Select first book
      component.onSelectBook(mockBookItems[0]);
      expect(component.selectedBook).toBe(mockBookItems[0]);

      // Select second book
      component.onSelectBook(mockBookItems[1]);
      expect(component.selectedBook).toBe(mockBookItems[1]);
      expect(mockBookService.getBookDetail$).toHaveBeenCalledTimes(2);

      component.bookDetail$?.subscribe(() => {
        done();
      });
    });
  });

  describe('Error Handling', () => {
    beforeEach(() => {
      fixture.detectChanges();
    });

    it('should log error to console when book loading fails', () => {
      spyOn(console, 'error');
      mockBookService.getAllBookItems$.and.returnValue(
        throwError(() => new Error('Network error'))
      );

      const errorFixture = TestBed.createComponent(AppComponent);
      errorFixture.detectChanges();

      expect(console.error).toHaveBeenCalledWith('Failed to load books:', jasmine.any(Error));
    });

    it('should log error to console when book detail loading fails', (done) => {
      spyOn(console, 'error');
      const selectedBook = mockBookItems[0];
      mockBookService.getBookDetail$.and.returnValue(
        throwError(() => new Error('Detail load error'))
      );

      component.onSelectBook(selectedBook);

      component.bookDetail$?.subscribe(() => {
        expect(console.error).toHaveBeenCalledWith('Failed to load book details:', jasmine.any(Error));
        done();
      });
    });
  });
});
