import { ChangeDetectorRef, Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { UserModel } from '../../../../models/user.model';
import { UserService } from '../../../../services/getUserService/user-service';
import { DatePipe } from '@angular/common';
import { RegisterPopup } from '../../register_pop-up/register-popup/register-popup';
import { PlayerRegister } from '../../playerRegister/player-register/player-register';
import { Reporting } from '../../reporting/reporting';

import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { FloatLabelModule } from 'primeng/floatlabel';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { DatePicker } from "primeng/datepicker";

import { Attendance } from '../../../../services/attendanceService/attendance';
import { AdminAttendance } from '../../../../models/attendance/attAdmin.model';
import { UpdateAttendance } from '../../../../models/attendance/attUpdate.model';


import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartOptions, ChartType } from 'chart.js';


@Component({
  selector: 'app-admin-screen',
  imports: [RegisterPopup, DatePipe,
    TableModule, InputTextModule,
    ButtonModule, IconFieldModule,
    InputIconModule,     ConfirmDialogModule,
    DialogModule, FloatLabelModule,
    SelectModule, FormsModule,
    BaseChartDirective, DatePicker,
    ToastModule,
    // For player Register
    PlayerRegister,
    // For Reporting
    Reporting
  ],
  providers: [ConfirmationService, MessageService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],

  templateUrl: './admin-screen.html',
  styleUrl: './admin-screen.scss',
})
export class AdminScreen implements OnInit {
  
  currentView: 'attendance' | 'users' | 'player' | 'report' = 'attendance'

  showRegister = false;
  openRegister() {
    this.showRegister = true;
  }
  closeRegister() {
    this.showRegister = false;
  }

  users: UserModel[] = [];

  constructor(
    private getUser: UserService,
    private confirmationService: ConfirmationService,
    private cdr: ChangeDetectorRef,

    private attendanceService: Attendance,
    private messageService: MessageService
    
  ){}

  ngOnInit(){
    this.loadUsers();

    this.loadAttendance();
  }

  loadUsers(): void {
    this.getUser.getUsers().subscribe({
      next: (data) => {
        this.users = data;
        this.cdr.detectChanges();
        // console.log("LoadUser",data);
      },
      error: (err) => {
        // console.error(err);
      }
    })
  }

  confirmDelete(user: any){
    this.confirmationService.confirm({
      header: 'Delete User',
      message: `Are you sure you want to delete: ${user.fullName}?`,
      icon: 'pi pi-exclamation-triangle',

      accept: () => {
        this.deleteUser(user.id);
      }
    })
  }

  deleteUser(id: number){
    // this.users = this.users.filter(user => user.id !== id)

    this.getUser.deleteUser(id).subscribe({
      next: () => {
        this.loadUsers();
      }
    });

  }

  displayEditDialog = false;

  selectedUser: any = {};

  roles = [
      { label: 'Admin', value: 'Admin' },
      { label: 'Normal', value: 'Normal' },
      // { label: 'HR', value: 'Hr' }
  ];

  editUser(user: any){

    this.selectedUser ={
      ...user
    }
    this.displayEditDialog = true;
  }

  saveUser() {

    this.getUser.updateUser(this.selectedUser.id, this.selectedUser).subscribe({
      next:()=> {
        this.loadUsers();
        this.displayEditDialog = false;
      },

      error: (error) => {
        this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: error?.error?.message || 'Unable to Load Players.'
        })
      }
      // error: (err) => console.error(err)
    })

  }


  // Attendance

  attendanceList: AdminAttendance[] = []
  attendanceloading = false
  selectedMonth: Date = new Date()

  loadAttendance(){
    this.attendanceloading = true
    this.cdr.detectChanges()

    const year = this.selectedMonth.getFullYear()
    const month = this.selectedMonth.getMonth() + 1

    this.attendanceService.getAttendanceHistory(year, month).subscribe({
      next: (data) => {
        this.attendanceList = data
        this.attendanceloading = false

        if (this.searchPlayerName.trim()) {
          this.searchPlayer()
        } else {
          this.cdr.detectChanges()
        }
      },
      error: (error) => {
        // console.error(err)
        this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: error?.error?.message || 'Unable to Load Players.'
        })
        
        this.attendanceloading = false
        this.cdr.detectChanges()
      }
    })
  }

  onMonthChange(){
    this.loadAttendance()
  }

  searchPlayerName: string = ''
  selectedPlayer: { name: string; belt: string; } | null = null
  presentDays: number = 0
  absentDays: number = 0

  public pieChartType: 'pie' = 'pie';

  public pieChartData: ChartData<'pie'> = {
    labels: [
      'Present',
      'Absent',
      // 'Leave',
    ],
    datasets: [
      {
        data: [0, 0],
         backgroundColor: [
          '#22c55e',
          '#ef4444'
          // '#1E3A8A',
          // '#2563EB',
          // // 'red',
          // '#5B8DEF',
          // // '#A5B8E1',
          // // '#D8DEE9'
        ],
        borderWidth:0
      }
    ]
  }

  public pieChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    maintainAspectRatio: false,

    plugins: {
      legend: {
        position: 'right',
      },
      tooltip: {
        // enabled: true,
        callbacks: {
          label: (context: any) => {

            const value = Number(context.raw);

            const data = context.dataset.data.map((d: any) => Number(d));

            const total = data.reduce((a: number, b: number) => a + b, 0);

            const percentage =
              total === 0 ? 0 : ((value / total) * 100).toFixed(1);

            return `${context.label}: ${percentage}%`;
          }
        }

      }
    }
  }  

  searchPlayer(){

    const playerName = this.searchPlayerName.trim().toLowerCase();

    if(!playerName){
      this.clearPlayer();
      return
    }

    const match = this.attendanceList.find(
      item => item.playerName.toLowerCase().includes(playerName)
    )

    if(!match){
      this.clearPlayer();
      return
    }

    const playerRecords = this.attendanceList.filter(
      item => item.playerName.toLowerCase() === match.playerName.toLowerCase()
    )

    this.selectedPlayer ={
      name: match.playerName,
      belt: match.playerBelt,
    }

    this.presentDays = playerRecords.filter(
      x => x.status === "Present"
    ).length;

    this.absentDays = playerRecords.filter(
      x => x.status === "Absent"
    ).length;

    this.updateChart();
    this.cdr.detectChanges();
  }

  updateChart(){
    this.pieChartData ={
      labels:[
        'Present',
        'Absent'
      ],

      datasets:[
        {
          data: [
            this.presentDays,
            this.absentDays
          ],
          backgroundColor: [
            '#22c55e',
            '#ef4444' 
          ],
          borderWidth: 0
        }
      ]
    }
    this.cdr.detectChanges();
  }

  clearPlayer(){
    this.selectedPlayer = null
    this.presentDays = 0
    this.absentDays = 0
    this.updateChart()
    this.cdr.detectChanges()
  }

  // For Attendance Update
  displayAttEditDialog = false
  selectedAttendance!: AdminAttendance
  updateAttendance: UpdateAttendance = {
    status: '',
    remarks: null
  };

  openAttEditDialog(attendance: AdminAttendance){
    
    this.selectedAttendance = attendance
    this.updateAttendance = {
      remarks: attendance.remarks,
      status: attendance.status
    }

    this.displayAttEditDialog = true
    this.cdr.detectChanges()
  }

  statusOptions = [
    { label: 'Present', value: 'Present' },
    { label: 'Absent', value: 'Absent' }
  ];

  saveEditAttendance() {

    this.attendanceService
        .updateAttendance(
            this.selectedAttendance.attendanceId,
            this.updateAttendance
        )
        .subscribe({
            next: (response) => {
                this.messageService.add({
                    severity: 'success',
                    summary: 'Success',
                    detail: response.message
                });
                this.displayAttEditDialog = false;
                // alert(response.message);
                this.loadAttendance();
                this.cdr.detectChanges();

            },

            error: (err) => {
                // alert(err.error.message);
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: err.error.message
                });
            }
        });
  }

}
