export class CreateTransactionDto {
  public fromAccountNumber: string;
  public fromAccountHolder: string;
  public toAccountNumber: string;
  public toAccountHolder: string;
  public amount: number;
  public description: string;

  constructor(init?: Partial<CreateTransactionDto>) {
    Object.assign(this, init);
  }
}

export class TransactionCreatedDto {
  public transactionId: string;
  public fromAccountName: string;
  public toAccountName: string;
  public newBalance: string;

  constructor(init?: Partial<TransactionCreatedDto>) {
    Object.assign(this, init);
  }
}

export class TransactionFailedDto {
  public fromAccountName: string;
  public toAccountName: string;
  public amount: string;
  public reason: string;

  constructor(init?: Partial<TransactionFailedDto>) {
    Object.assign(this, init);
  }
}
