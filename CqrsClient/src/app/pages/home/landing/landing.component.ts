import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import {
  CreateTransactionDto,
  TransactionCreatedDto,
  TransactionFailedDto
} from 'src/app/models/transaction.models';
import { TransactionService } from 'src/app/services/transaction.service';
import { MessageService } from 'primeng/api';

import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit {
  transactionForm: FormGroup;
  private hubConnection: HubConnection | undefined;
  constructor(
    private service: TransactionService,
    private messageService: MessageService
  ) {}

  private constructForm() {
    this.transactionForm = new FormGroup({
      fromAccountNumber: new FormControl('1047562346', [Validators.required]),
      fromAccountHolder: new FormControl('Steven Segal', [Validators.required]),
      toAccountNumber: new FormControl('239654876', [Validators.required]),
      toAccountHolder: new FormControl('Sylvester Stallone', [
        Validators.required
      ]),
      amount: new FormControl('', [Validators.required]),
      description: new FormControl('', [Validators.required])
    });
  }

  public connectToSignalR() {
    const self = this;
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`https://localhost:5003/hubs/transactions`)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        self.hubConnection.on(
          'transaction-created',
          (message: TransactionCreatedDto) => {
            this.messageService.add({
              severity: 'success',
              summary: 'Transaction succeeded',
              detail: `The transaction from ${message.fromAccountName} to ${message.toAccountName} succeeded, your new balance is ${message.newBalance}`
            });
          }
        );
        self.hubConnection.on(
          'transaction-failed',
          (message: TransactionFailedDto) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Transaction failed',
              detail: `The transaction from ${message.fromAccountName} to ${message.toAccountName} failed with reason ${message.reason}`
            });
          }
        );
      })
      .catch(err => console.error(err.toString()));
  }

  createTransaction() {
    const transaction = new CreateTransactionDto(this.transactionForm.value);
    this.service.createTransaction(transaction).subscribe(val => {
      if (val) {
        this.messageService.add({
          severity: 'info',
          summary: 'Transaction sent',
          detail:
            "The transaction was sent to our server, please wait while it's being processed"
        });
      }
    });
  }

  ngOnInit() {
    this.constructForm();
    this.connectToSignalR();
  }
}
