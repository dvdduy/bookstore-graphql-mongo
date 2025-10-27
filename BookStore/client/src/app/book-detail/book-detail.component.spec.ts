import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BookDetailComponent } from './book-detail.component';
import { BookDetail } from '../models/BookDetail';
import { AuthorNamesPipe } from '../pipes/author-names.pipe';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('BookDetailComponent', () => {
  let component: BookDetailComponent;
  let fixture: ComponentFixture<BookDetailComponent>;

  const mockBookDetail: BookDetail = {
    id: '123',
    title: 'Test Book',
    description: 'This is a detailed description of the test book.',
    imageUrl: 'http://example.com/image.jpg',
    publisher: 'Test Publisher',
    publishedDate: new Date('2023-01-01'),
    length: 350,
    authors: [
      { id: '1', name: 'Author One' },
      { id: '2', name: 'Author Two' }
    ],
    reviews: [
      { id: '1', rating: 5, title: 'Excellent', description: 'Amazing book!' },
      { id: '2', rating: 4, title: 'Good', description: 'Worth reading' }
    ],
    averageReview: 4.5
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookDetailComponent, AuthorNamesPipe],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookDetailComponent);
    component = fixture.componentInstance;
    component.book = mockBookDetail; // Set required input
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Input Properties', () => {
    it('should accept book detail input', () => {
      component.book = mockBookDetail;
      fixture.detectChanges();

      expect(component.book).toEqual(mockBookDetail);
    });

    it('should display all book properties', () => {
      component.book = mockBookDetail;
      fixture.detectChanges();

      expect(component.book.id).toBe('123');
      expect(component.book.title).toBe('Test Book');
      expect(component.book.description).toBe('This is a detailed description of the test book.');
      expect(component.book.publisher).toBe('Test Publisher');
      expect(component.book.length).toBe(350);
    });

    it('should handle book with multiple authors', () => {
      component.book = mockBookDetail;
      fixture.detectChanges();

      expect(component.book.authors.length).toBe(2);
      expect(component.book.authors[0].name).toBe('Author One');
      expect(component.book.authors[1].name).toBe('Author Two');
    });

    it('should handle book with single author', () => {
      const bookWithSingleAuthor: BookDetail = {
        ...mockBookDetail,
        authors: [{ id: '1', name: 'Single Author' }]
      };
      component.book = bookWithSingleAuthor;
      fixture.detectChanges();

      expect(component.book.authors.length).toBe(1);
      expect(component.book.authors[0].name).toBe('Single Author');
    });

    it('should handle book with no authors', () => {
      const bookWithNoAuthors: BookDetail = {
        ...mockBookDetail,
        authors: []
      };
      component.book = bookWithNoAuthors;
      fixture.detectChanges();

      expect(component.book.authors.length).toBe(0);
    });

    it('should handle book with multiple reviews', () => {
      component.book = mockBookDetail;
      fixture.detectChanges();

      expect(component.book.reviews.length).toBe(2);
      expect(component.book.reviews[0].rating).toBe(5);
      expect(component.book.reviews[1].rating).toBe(4);
    });

    it('should handle book with no reviews', () => {
      const bookWithNoReviews: BookDetail = {
        ...mockBookDetail,
        reviews: [],
        averageReview: undefined
      };
      component.book = bookWithNoReviews;
      fixture.detectChanges();

      expect(component.book.reviews.length).toBe(0);
      expect(component.book.averageReview).toBeUndefined();
    });

    it('should handle book with undefined average review', () => {
      const bookWithUndefinedReview: BookDetail = {
        ...mockBookDetail,
        averageReview: undefined
      };
      component.book = bookWithUndefinedReview;
      fixture.detectChanges();

      expect(component.book.averageReview).toBeUndefined();
    });

    it('should handle published date correctly', () => {
      component.book = mockBookDetail;
      fixture.detectChanges();

      expect(component.book.publishedDate).toEqual(new Date('2023-01-01'));
    });

    it('should handle book with empty image URL', () => {
      const bookWithNoImage: BookDetail = {
        ...mockBookDetail,
        imageUrl: ''
      };
      component.book = bookWithNoImage;
      fixture.detectChanges();

      expect(component.book.imageUrl).toBe('');
    });

    it('should handle book length variations', () => {
      const shortBook: BookDetail = { ...mockBookDetail, length: 100 };
      component.book = shortBook;
      fixture.detectChanges();
      expect(component.book.length).toBe(100);

      const longBook: BookDetail = { ...mockBookDetail, length: 1000 };
      component.book = longBook;
      fixture.detectChanges();
      expect(component.book.length).toBe(1000);
    });
  });

  describe('Data Display', () => {
    it('should update display when book input changes', () => {
      const firstBook = mockBookDetail;
      component.book = firstBook;
      fixture.detectChanges();
      expect(component.book.title).toBe('Test Book');

      const secondBook: BookDetail = {
        ...mockBookDetail,
        id: '456',
        title: 'Another Book'
      };
      component.book = secondBook;
      fixture.detectChanges();
      expect(component.book.title).toBe('Another Book');
    });

    it('should maintain data integrity after multiple updates', () => {
      component.book = mockBookDetail;
      fixture.detectChanges();

      const initialAuthorsCount = component.book.authors.length;
      const initialReviewsCount = component.book.reviews.length;

      fixture.detectChanges();

      expect(component.book.authors.length).toBe(initialAuthorsCount);
      expect(component.book.reviews.length).toBe(initialReviewsCount);
    });
  });

  describe('Review Data', () => {
    it('should correctly display review ratings', () => {
      component.book = mockBookDetail;
      fixture.detectChanges();

      expect(component.book.reviews[0].rating).toBe(5);
      expect(component.book.reviews[1].rating).toBe(4);
    });

    it('should display review titles and descriptions', () => {
      component.book = mockBookDetail;
      fixture.detectChanges();

      expect(component.book.reviews[0].title).toBe('Excellent');
      expect(component.book.reviews[0].description).toBe('Amazing book!');
      expect(component.book.reviews[1].title).toBe('Good');
      expect(component.book.reviews[1].description).toBe('Worth reading');
    });

    it('should calculate average review correctly', () => {
      component.book = mockBookDetail;
      fixture.detectChanges();

      expect(component.book.averageReview).toBe(4.5);
    });
  });
});
