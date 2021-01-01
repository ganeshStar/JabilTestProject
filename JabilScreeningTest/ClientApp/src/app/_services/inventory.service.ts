import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models';
import { apiUrl } from '../../apiSettings';
import { catchError, first, map, retry } from 'rxjs/operators';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { Inventory } from '../_models/Inventory';
import { CommonHandler } from './commonHandler';

@Injectable({ providedIn: 'root' })

export class InventoryService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
  private headers = {};

  constructor(private http: HttpClient, private handler: CommonHandler) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.headers = { 'Authorization': 'Bearer ' + this.currentUserSubject.value.token };
  }

  getAllProducts() {
    return this.http.get<Inventory[]>(apiUrl.baseUrl + apiUrl.getAllProductsUrl, this.headers).pipe(retry(1),
      catchError(this.handler.errorHandler));
  }

  getProductToEdit(productId) {

    return this.http.get<Inventory>(apiUrl.baseUrl + apiUrl.getProductUrl + '?id=' + productId, this.headers).pipe(retry(1),
      catchError(this.handler.errorHandler));
  }

  addProduct(model): Observable<Inventory> {
    return this.http.post<Inventory>(apiUrl.baseUrl + apiUrl.addProductUrl, model, this.headers).pipe(retry(1),
      catchError(this.handler.errorHandler));
  }

  updateProduct(model): Observable<Inventory> {
    return this.http.post<Inventory>(apiUrl.baseUrl + apiUrl.updateProductUrl, model, this.headers).pipe(retry(1),
      catchError(this.handler.errorHandler));
  }
}
