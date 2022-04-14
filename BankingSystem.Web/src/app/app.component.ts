import { Component } from '@angular/core';
import { BankAccount } from './models/bank-account.model'
import { BankAccountService } from './services/bank-account.service'
     
@Component({
    selector: 'my-app',
    template: `<label>Введите имя:</label>
                 <input [(ngModel)]="name" placeholder="name">
                 <h1>Добро пожаловать {{name}}!</h1>`
})
export class AppComponent { 
    
    bankAccounts: BankAccount[] = [];

    constructor(private bankAccountService: BankAccountService) {

    }

}