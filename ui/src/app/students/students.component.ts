import { Component, OnInit ,ViewChild} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { StudentUI } from '../models/ui-models/studentui.model';
import { StudentsService } from './students.service';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css']
})
export class StudentsComponent implements OnInit {
  students:StudentUI[]=[];

  //related to mat table
  displayedColumns: string[] =['firstName', 'lastName', 'dateOfBirth', 'email', 'mobile', 'gender','edit'];
  dataSource!: MatTableDataSource<StudentUI>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private service:StudentsService) { }

  ngOnInit(): void {
    this.service.getStudents().subscribe(
      (res)=>{
        this.students=res;
        this.dataSource = new MatTableDataSource<StudentUI>(this.students);
        if (this.paginator) {
          this.dataSource.paginator = this.paginator;
        }
    
        if (this.sort) {
          this.dataSource.sort = this.sort;
        }
      },
      (err)=>{console.log(err)}
    );
  }

 

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}
