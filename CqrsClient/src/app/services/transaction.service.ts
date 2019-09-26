import { Injectable } from '@angular/core';
import { CreateTransactionDto } from '../models/transaction.models';
import { Observable } from 'rxjs';
import { HttpResponse, HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  constructor(private http: HttpClient) {}

  public createTransaction(dto: CreateTransactionDto): Observable<boolean> {
    return this.http.post<boolean>(
      'https://localhost:5003/api/transactions',
      dto
    );
  }
}
