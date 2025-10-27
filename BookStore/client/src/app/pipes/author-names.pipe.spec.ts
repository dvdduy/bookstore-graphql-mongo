import { AuthorNamesPipe } from './author-names.pipe';
import { Author } from '../models/Author';

describe('AuthorNamesPipe', () => {
  let pipe: AuthorNamesPipe;

  beforeEach(() => {
    pipe = new AuthorNamesPipe();
  });

  it('should create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  describe('Transform', () => {
    it('should convert array of authors to comma-separated string', () => {
      const authors: Author[] = [
        { id: '1', name: 'Author One' },
        { id: '2', name: 'Author Two' },
        { id: '3', name: 'Author Three' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe('Author One, Author Two, Author Three');
    });

    it('should handle single author', () => {
      const authors: Author[] = [
        { id: '1', name: 'Single Author' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe('Single Author');
    });

    it('should handle two authors', () => {
      const authors: Author[] = [
        { id: '1', name: 'First Author' },
        { id: '2', name: 'Second Author' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe('First Author, Second Author');
    });

    it('should return empty string for empty array', () => {
      const authors: Author[] = [];

      const result = pipe.transform(authors);

      expect(result).toBe('');
    });

    it('should return empty string for null value', () => {
      const result = pipe.transform(null as any);

      expect(result).toBe('');
    });

    it('should return empty string for undefined value', () => {
      const result = pipe.transform(undefined as any);

      expect(result).toBe('');
    });

    it('should handle authors with special characters in names', () => {
      const authors: Author[] = [
        { id: '1', name: "O'Brien" },
        { id: '2', name: 'Jean-Paul Sartre' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe("O'Brien, Jean-Paul Sartre");
    });

    it('should handle authors with long names', () => {
      const authors: Author[] = [
        { id: '1', name: 'Johann Wolfgang von Goethe' },
        { id: '2', name: 'Fyodor Mikhailovich Dostoevsky' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe('Johann Wolfgang von Goethe, Fyodor Mikhailovich Dostoevsky');
    });

    it('should handle authors with unicode characters', () => {
      const authors: Author[] = [
        { id: '1', name: 'José Saramago' },
        { id: '2', name: 'Günter Grass' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe('José Saramago, Günter Grass');
    });

    it('should preserve spacing in author names', () => {
      const authors: Author[] = [
        { id: '1', name: 'First  Name' }, // double space
        { id: '2', name: 'Second Name' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe('First  Name, Second Name');
    });

    it('should handle many authors (5+)', () => {
      const authors: Author[] = [
        { id: '1', name: 'Author 1' },
        { id: '2', name: 'Author 2' },
        { id: '3', name: 'Author 3' },
        { id: '4', name: 'Author 4' },
        { id: '5', name: 'Author 5' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe('Author 1, Author 2, Author 3, Author 4, Author 5');
    });
  });

  describe('Edge Cases', () => {
    it('should handle authors with empty name strings', () => {
      const authors: Author[] = [
        { id: '1', name: '' },
        { id: '2', name: 'Valid Name' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe(', Valid Name');
    });

    it('should handle authors with whitespace-only names', () => {
      const authors: Author[] = [
        { id: '1', name: '   ' },
        { id: '2', name: 'Valid Name' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe('   , Valid Name');
    });

    it('should not modify original array', () => {
      const authors: Author[] = [
        { id: '1', name: 'Author One' },
        { id: '2', name: 'Author Two' }
      ];
      const originalLength = authors.length;

      pipe.transform(authors);

      expect(authors.length).toBe(originalLength);
      expect(authors[0].name).toBe('Author One');
      expect(authors[1].name).toBe('Author Two');
    });
  });

  describe('Separator Format', () => {
    it('should use comma and space as separator', () => {
      const authors: Author[] = [
        { id: '1', name: 'First' },
        { id: '2', name: 'Second' }
      ];

      const result = pipe.transform(authors);

      expect(result).toBe('First, Second');
      expect(result).toContain(', ');
      expect(result).not.toContain(',Second'); // No space missing before Second
    });

    it('should not have leading or trailing separators', () => {
      const authors: Author[] = [
        { id: '1', name: 'First' },
        { id: '2', name: 'Second' }
      ];

      const result = pipe.transform(authors);

      expect(result).not.toMatch(/^,/);
      expect(result).not.toMatch(/,$/);
    });
  });
});

