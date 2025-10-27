import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {ApolloModule, APOLLO_OPTIONS} from 'apollo-angular';
import {HttpLink} from 'apollo-angular/http';
import {InMemoryCache} from '@apollo/client/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BookListComponent } from './book-list/book-list.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AuthorNamesPipe } from './pipes/author-names.pipe';
import { BookItemComponent } from './book-item/book-item.component';
import { BookDetailComponent } from './book-detail/book-detail.component';

@NgModule({ declarations: [
        AppComponent,
        BookListComponent,
        AuthorNamesPipe,
        BookItemComponent,
        BookDetailComponent
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        AppRoutingModule,
        ApolloModule], providers: [
        {
            provide: APOLLO_OPTIONS,
            useFactory: (httpLink: HttpLink) => {
                return {
                    cache: new InMemoryCache(),
                    link: httpLink.create({
                        uri: 'http://localhost:4200/graphql'
                    })
                };
            },
            deps: [HttpLink]
        },
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule { }
