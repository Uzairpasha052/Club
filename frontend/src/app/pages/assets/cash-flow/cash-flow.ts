import { ChangeDetectorRef, Component } from '@angular/core';
import { FormsModule, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { SelectModule } from 'primeng/select';
import { InputNumberModule } from 'primeng/inputnumber';
import { DatePicker } from 'primeng/datepicker';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';

import { CashService } from '../../../services/cashFlowService/cash-service';
import { CashInModel } from '../../../models/CashFlow/cashIn.model';
import { PlayerService } from '../../../services/playerService/player';
import { CashOutModel } from '../../../models/CashFlow/cashOut.model';

@Component({
  selector: 'app-cash-flow',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ToastModule,
    SelectModule,
    InputNumberModule,
    DatePicker,
    ButtonModule,
    TableModule,
    InputTextModule
  ],
  templateUrl: './cash-flow.html',
  styleUrl: './cash-flow.scss',
  providers: [MessageService]
})
export class CashFlow {

    currentView: 'cashIn' | 'cashOut' = 'cashIn'

    setView(view: 'cashIn' | 'cashOut'): void {
        this.currentView = view;
        this.cdr.detectChanges();
    }

    cashInForm!: FormGroup;
    cashOutForm!: FormGroup;


    players: any[] = [];
    cashInList: any[] = [];
    selectedMonth: Date = new Date();

    cashOutList: any[] = [];

    saving = false;

    constructor(
        private fb: FormBuilder,
        private cashInService: CashService,
        private playerService: PlayerService,
        private messageService: MessageService,
        private cdr: ChangeDetectorRef
    ) {}


    ngOnInit(): void {

        this.createForm();
        this.loadPlayers();
        this.loadCashIn();

        this.loadCashOut();
    }


    // =========================
    // Create Form
    // =========================

    createForm(): void {

        this.cashInForm = this.fb.group({

            playerId: [
                null,
                Validators.required
            ],
            amount: [
                null,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            date: [
                new Date(),
                Validators.required
            ]

        });

        this.cashOutForm = this.fb.group({

            description: [
                null,
                Validators.required
            ],
            amount: [
                null,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            date: [
                new Date(),
                Validators.required
            ]

        });

    }


    // =========================
    // Load Players
    // =========================

    loadPlayers(): void {

        this.playerService.getPlayers().subscribe({

            next: (response: any) => {
                this.players = (response || []).filter((player: any) =>
                    player.stdStatus?.toLowerCase() === 'active'
                );
                this.cdr.detectChanges();
            },

            error: (error) => {
                // console.error('Error loading players:', error);
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error?.error?.message || 'Unable to Load Players.'
                });

            }

        });

    }


    // =========================
    // Load Cash In
    // =========================

    loadCashIn(): void {
        const year = this.selectedMonth.getFullYear();
        const month = this.selectedMonth.getMonth() + 1;

        this.cashInService.GetAllCashIn(year, month).subscribe({
            next: (response: any) => {
                this.cashInList = response;
                this.cdr.detectChanges();
            },
            error: (error) => {
                // console.error('Error loading cash in:', error);
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error?.error?.message || 'Unable to Load cash in.'
                });
                
            }
        });
    }

    // =========================
    // Load Cash In
    // =========================

    loadCashOut(): void {
        const year = this.selectedMonth.getFullYear();
        const month = this.selectedMonth.getMonth() + 1;

        this.cashInService.GetAllCashOut(year, month).subscribe({
            next: (response: any) => {
                this.cashOutList = response;
                this.cdr.detectChanges();
            },
            error: (error) => {
                // console.error('Error loading cash in:', error);
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error?.error?.message || 'Unable to Load cash in.'
                });
            }
        });
    }

    onMonthChange(): void {
        this.loadCashIn();
        this.loadCashOut();
    }


    // =========================
    // Save Cash In
    // =========================

    saveCashIn(): void {

        if (this.cashInForm.invalid) {

            this.cashInForm.markAllAsTouched();

            return;
        }


        this.saving = true;


        const formValue = this.cashInForm.value;


        const cashIn: CashInModel = {
            playerId: formValue.playerId,
            amount: formValue.amount,
            paymentDate: this.formatDate(formValue.date) //formValue.date
        };


        this.cashInService.CreateCashIn(cashIn).subscribe({

            next: () => {
                this.saving = false;

                this.cashInForm.reset({
                    playerId: null,
                    amount: null,
                    date: new Date()
                });

                this.messageService.add({
                    severity: 'success',
                    summary: 'Saved',
                    detail: 'Cash in saved successfully.'
                });

                this.loadCashIn();
                this.cdr.detectChanges();
            },

            error: (error) => {
                this.saving = false;
                // console.error('Error saving cash in:', error);
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error?.error?.message || 'Unable to save cash in.'
                });
                this.cdr.detectChanges();
            }

        });

    }

    // =========================
    // Save Cash Out
    // =========================

    saveCashOut(): void {

        if (this.cashOutForm.invalid) {

            this.cashOutForm.markAllAsTouched();

            return;
        }


        this.saving = true;


        const formValue = this.cashOutForm.value;


        const cashOut: CashOutModel = {
            amount: formValue.amount,
            description: formValue.description,
            paymentDate: this.formatDate(formValue.date) //formValue.date
        };


        this.cashInService.CreateCashOut(cashOut).subscribe({

            next: () => {
                this.saving = false;

                this.cashOutForm.reset({
                    amount: null,
                    description: null,
                    date: new Date()
                });

                this.messageService.add({
                    severity: 'success',
                    summary: 'Saved',
                    detail: 'Cash Out saved successfully.'
                });

                this.loadCashOut();
                this.cdr.detectChanges();
            },

            error: (error) => {
                this.saving = false;
                // console.error('Error saving cash Out:', error);
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error?.error?.message || 'Unable to save cash Out.'
                });
                this.cdr.detectChanges();
            }

        });

    }


    // =========================
    // Format Date
    // =========================

    private formatDate(date: Date): string {

        const year = date.getFullYear();

        const month = String(
            date.getMonth() + 1
        ).padStart(2, '0');

        const day = String(
            date.getDate()
        ).padStart(2, '0');


        return `${year}-${month}-${day}`;

    }

}