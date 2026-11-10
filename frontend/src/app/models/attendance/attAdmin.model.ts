

export interface AdminAttendance {

  attendanceId: number;
  playerId: number;
  playerName: string;
  playerBelt: string;
  attendanceDate: string;
  status: string;
  remarks: string | null;
 
}
