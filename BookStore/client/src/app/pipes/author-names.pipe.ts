import { Pipe, PipeTransform } from "@angular/core";
import { Author } from "../models/Author";

@Pipe({name: 'authorNames'})
export class AuthorNamesPipe implements PipeTransform {
    transform(value: Author[], ...args: any[]) {        
        return !!value 
        ? value.map(x=>x.name).reduce((previous, current) => previous + ", " + current)
        : '';
    }

}