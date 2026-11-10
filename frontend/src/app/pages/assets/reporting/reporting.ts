import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePickerModule } from 'primeng/datepicker';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { SelectModule } from 'primeng/select';

import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

import { ReportService } from '../../../services/reportService/report-service';

@Component({
  selector: 'app-reporting',
  imports: [FormsModule, DatePickerModule,
    ButtonModule, CardModule, SelectModule, ToastModule],
  templateUrl: './reporting.html',
  styleUrl: './reporting.scss',
  providers: [MessageService]

})
export class Reporting {

  selectedStatus: string = 'all'

  statusOptions = [
    { label: 'All Players', value: 'all' },
    { label: 'Active Players', value: 'active' },
    { label: 'Inactive Players', value: 'inactive' }
  ];

  // Attendance Report
  startDate: Date | null = null
  endDate: Date | null = null

  // Cash In
  cashInStartDate: Date | null = null;
  cashInEndDate: Date | null = null;
  // Cash Out
  cashOutStartDate: Date | null = null;
  cashOutEndDate: Date | null = null;
  // Cash Flow
  cashFlowStartDate: Date | null = null;
  cashFlowEndDate: Date | null = null;


  constructor(
    private reportService: ReportService,
    private messageService: MessageService
  ){}

  generatePlayerRecordReport(): void {
    this.reportService.getPlayerRecordReport(this.selectedStatus)
    .subscribe({
      next: (blob: Blob) => {
        this.openPdf(blob, `Player-Record-Report-${this.selectedStatus}.pdf`)
      },
      error: (error) => {
        // console.error('Error generating player record report:', error);
        this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error?.error?.message || 'Unable to generate player record report.'
          });
      }
    })
  }

  generatePlayerReasonReport():void {
    this.reportService.getPlayerReasonReport()
    .subscribe({
      next: (blob: Blob) => {
        this.openPdf(blob, 'Player-Reason-Report.pdf')
      },
      error: (error) => {
        // console.error('Error generating player reason report:', error)
        alert('Unable to generate player reason report.');
        this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error?.error?.message || 'Unable to generate player reason report.'
          });
      }
    })
  }

  generatePlayerAttendanceReport(): void {
    if (!this.startDate || !this.endDate) {
      this.messageService.add({
        severity: 'error',
        summary: 'Issue',
        detail: 'Please select both start and end dates.'
      });
      return;
    }

    if (this.startDate > this.endDate) {
      this.messageService.add({
        severity: 'error',
        summary: 'Issue',
        detail: 'StartDate cannot be grater than EndDate'
      });
      return;
    }

    const startDate = this.attFormatDate(this.startDate);
    const endDate = this.attFormatDate(this.endDate);

    this.reportService.getPlayerAttendanceReport(startDate,endDate)
    .subscribe({
      next: (blob: Blob) => {
        this.openPdf(blob, 'Player-Attendance-Report.pdf')
      },
      error: (error) => {
        // console.error('Error generating player reason report:', error)
        this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error?.error?.message || 'Unable to generate Player Attendance report.'
          });
      }
    })

    
  }
 

  generateCashInReport(): void {
    
    if(!this.cashInStartDate || !this.cashInEndDate){
      this.messageService.add({
        severity: 'error',
        summary: 'Issue',
        detail: 'Please select both start and end dates.'
      });
      return;
    }

    if (this.cashInStartDate > this.cashInEndDate) {
      this.messageService.add({
        severity: 'error',
        summary: 'Issue',
        detail: 'StartDate cannot be grater than EndDate'
      });
      return;
    }

    const startDate = this.attFormatDate(
      this.cashInStartDate
    );

    const endDate = this.attFormatDate(
      this.cashInEndDate
    );

    this.reportService.getCashInReport(startDate, endDate)
      .subscribe({
        next: (blob: Blob) => {
          this.openPdf(blob, 'Player-CashIn-Report.pdf')
        },
        error: (error) => {
          // console.error('Error generating Cash In report:', error)
          this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error?.error?.message || 'Unable to generate club Cash-In report.'
          });
        }
      })

  }

  generateCashOutReport(): void {

    if (!this.cashOutStartDate || !this.cashOutEndDate) {
      this.messageService.add({
          severity: 'error',
          summary: 'Issue',
          detail: 'Please select both start and end dates.'
        });
      return;
    }

    if (this.cashOutStartDate > this.cashOutEndDate) {
      this.messageService.add({
          severity: 'error',
          summary: 'Issue',
          detail: 'StartDate cannot be grater than EndDate'
        });
      return;
    }

    const startDate = this.attFormatDate(
      this.cashOutStartDate
    );

    const endDate = this.attFormatDate(
      this.cashOutEndDate
    );

    this.reportService
      .getCashOutReport(startDate, endDate)
      .subscribe({
        next: (blob: Blob) => {
          this.openPdf(blob, 'Player-CashOut-Report.pdf')
        },
        error: (error) => {
          // console.error('Error generating Cash Out report:', error);
          this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error?.error?.message || 'Unable to generate club Cash-Out report.'
          });
        }
      });
  }

  generateCashFlowReport(): void {

    if (!this.cashFlowStartDate || !this.cashFlowEndDate) {
      this.messageService.add({
          severity: 'error',
          summary: 'Issue',
          detail: 'Please select both start and end dates.'
        });
      return;
    }

     if (this.cashFlowStartDate > this.cashFlowEndDate) {
      this.messageService.add({
          severity: 'error',
          summary: 'Issue',
          detail: 'StartDate cannot be grater than EndDate'
        });
      return;
    }

    const startDate = this.attFormatDate(
      this.cashFlowStartDate
    );

    const endDate = this.attFormatDate(
      this.cashFlowEndDate  
    );

    this.reportService
      .getCashFlowReport(startDate, endDate)
      .subscribe({
        next: (blob: Blob) => {
          this.openPdf(blob, 'Club-CashFlow-Report.pdf')
        },
        error: (error) => {
          // console.error('Error generating Cash Flow report:', error);
          this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error?.error?.message || 'Unable to generate club Cash-Flow report.'
          });
        }
      });

  }

  private attFormatDate(date: Date): string {

    const year = date.getFullYear();

    const month = String(
      date.getMonth() + 1
    ).padStart(2, '0');

    const day = String(
      date.getDate()
    ).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private openPdf(blob: Blob, fileName: string): void {
    const pdfName = fileName.toLowerCase().endsWith('.pdf')
      ? fileName
      : `${fileName}.pdf`;

    const pdfFile = new File([blob], pdfName, { type: 'application/pdf' });
    const fileURL = URL.createObjectURL(pdfFile);

    const link = document.createElement('a');
    link.href = fileURL;
    link.download = pdfName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    window.open(fileURL, '_blank');

    setTimeout(() => {
      URL.revokeObjectURL(fileURL);
    }, 60000);
  }



}
