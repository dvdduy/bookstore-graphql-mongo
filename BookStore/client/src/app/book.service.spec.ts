import { TestBed } from '@angular/core/testing';
import { ApolloTestingModule, ApolloTestingController } from 'apollo-angular/testing';
import { BookService } from './book.service';
import { BookItem } from './models/BookItem';
import { BookDetail } from './models/BookDetail';

describe('BookService', () => {
  let service: BookService;
  let controller: ApolloTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ApolloTestingModule],
      providers: [BookService]
    });
    service = TestBed.inject(BookService);
    controller = TestBed.inject(ApolloTestingController);
  });

  afterEach(() => {
    controller.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAllBookItems$', () => {
    it('should return list of books', (done) => {
      const mockBooks: BookItem[] = [
        {
          id: '1',
          title: 'Test Book 1',
          publishedDate: new Date('2023-01-01'),
          publisher: 'Test Publisher',
          authors: [{ id: '1', name: 'Author 1' }],
          averageReview: 4.5
        },
        {
          id: '2',
          title: 'Test Book 2',
          publishedDate: new Date('2023-02-01'),
          publisher: 'Test Publisher 2',
          authors: [{ id: '2', name: 'Author 2' }],
          averageReview: 4.0
        }
      ];

      service.getAllBookItems$().subscribe({
        next: (books) => {
          expect(books.length).toBe(2);
          expect(books[0].id).toBe('1');
          expect(books[0].title).toBe('Test Book 1');
          expect(books[1].id).toBe('2');
          expect(books[1].title).toBe('Test Book 2');
          done();
        },
        error: done.fail
      });

      const op = controller.expectOne((operation) => {
        return operation.query.definitions.some((def: any) => 
          def.selectionSet?.selections?.some((sel: any) => 
            sel.name?.value === 'books'
          )
        );
      });

      expect(op.operation.variables).toEqual({});

      op.flush({
        data: {
          books: mockBooks
        }
      });
    });

    it('should handle empty book list', (done) => {
      service.getAllBookItems$().subscribe({
        next: (books) => {
          expect(books).toEqual([]);
          expect(books.length).toBe(0);
          done();
        },
        error: done.fail
      });

      const op = controller.expectOne((operation) => {
        return operation.query.definitions.some((def: any) => 
          def.selectionSet?.selections?.some((sel: any) => 
            sel.name?.value === 'books'
          )
        );
      });

      op.flush({
        data: {
          books: []
        }
      });
    });
  });

  describe('getBookDetail$', () => {
    it('should return book details for valid ID', (done) => {
      const mockBookDetail: BookDetail = {
        id: '123',
        title: 'Detailed Book',
        description: 'A detailed description',
        imageUrl: 'http://example.com/image.jpg',
        publisher: 'Test Publisher',
        publishedDate: new Date('2023-01-01'),
        length: 300,
        authors: [{ id: '1', name: 'Author 1' }],
        reviews: [
          { id: '1', rating: 5, title: 'Great', description: 'Amazing book' }
        ],
        averageReview: 5.0
      };

      service.getBookDetail$('123').subscribe({
        next: (book) => {
          expect(book).toEqual(mockBookDetail);
          expect(book.id).toBe('123');
          expect(book.title).toBe('Detailed Book');
          expect(book.reviews.length).toBe(1);
          done();
        },
        error: done.fail
      });

      const op = controller.expectOne((operation) => {
        return operation.query.definitions.some((def: any) => 
          def.selectionSet?.selections?.some((sel: any) => 
            sel.name?.value === 'book'
          )
        );
      });

      expect(op.operation.variables).toEqual({ id: '123' });

      op.flush({
        data: {
          book: mockBookDetail
        }
      });
    });

    it('should use variables for ID parameter to prevent injection', (done) => {
      const bookId = '456';

      service.getBookDetail$(bookId).subscribe({
        next: () => done(),
        error: done.fail
      });

      const op = controller.expectOne((operation) => {
        return operation.query.definitions.some((def: any) => 
          def.selectionSet?.selections?.some((sel: any) => 
            sel.name?.value === 'book'
          )
        );
      });

      // Verify that variables are used (not string interpolation)
      expect(op.operation.variables).toEqual({ id: bookId });
      expect(op.operation.variables['id']).toBe('456');

      op.flush({
        data: {
          book: {
            id: bookId,
            title: 'Test',
            description: 'Test',
            imageUrl: '',
            publisher: 'Test',
            publishedDate: new Date(),
            length: 100,
            authors: [],
            reviews: [],
            averageReview: null
          }
        }
      });
    });

    it('should handle book not found', (done) => {
      service.getBookDetail$('999').subscribe({
        next: (book) => {
          expect(book).toBeNull();
          done();
        },
        error: done.fail
      });

      const op = controller.expectOne((operation) => {
        return operation.query.definitions.some((def: any) => 
          def.selectionSet?.selections?.some((sel: any) => 
            sel.name?.value === 'book'
          )
        );
      });

      op.flush({
        data: {
          book: null
        }
      });
    });
  });
});
