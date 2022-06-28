import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { GenderUI } from 'src/app/models/ui-models/genderui.model';
import { StudentUI } from 'src/app/models/ui-models/studentui.model';
import { GenderService } from 'src/app/services/gender.service';
import { StudentsService } from '../students.service';

@Component({
  selector: 'app-view-student',
  templateUrl: './view-student.component.html',
  styleUrls: ['./view-student.component.css']
})
export class ViewStudentComponent implements OnInit {
  studentId: string | null | undefined;
  genderList: GenderUI[] = [];
  isNewStudent = false;
  header = '';
  displayProfileImageUrl = '';
  student: StudentUI = {
    id: '',
    firstName: '',
    lastName: '',
    dateOfBirth: '',
    email: '',
    mobile: 0,
    genderId: '',
    profileImageUrl: '',
    gender: {
      id: '',
      description: ''
    },
    address: {
      id: '',
      physicalAddress: '',
      postalAddress: ''
    }
  };
  @ViewChild('studentDetailsForm') studentDetailsForm?: NgForm;
  constructor(private service: StudentsService,
    private genderService: GenderService,
    private route: ActivatedRoute,
    private snack: MatSnackBar,
    private router: Router) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.studentId = params.get('id');
      if (this.studentId) {


        if (this.studentId.toLowerCase() === 'Add'.toLowerCase()) {
          // -> new Student Functionality
          this.isNewStudent = true;
          this.header = 'Add New Student';
          this.setImage();
        } else {
          // -> Existing Student Functionality
          this.isNewStudent = false;
          this.header = 'Edit Student';
          this.service.getStudent(this.studentId).subscribe(
            (res) => { this.student = res; this.setImage(); },
            (err) => { this.setImage(); }
          );
        }
        this.genderService.getGenderList().subscribe(
          (res) => { this.genderList = res; }
        );

      }
    });
  }

  onUpdate(): void {
    if (this.studentDetailsForm?.form.valid) {
      this.service.updateStudent(this.student.id, this.student)
        .subscribe(
          (res) => {
            this.snack.open('successfully updated', undefined, { duration: 2000 })
          },
          (err) => {
            this.snack.open( JSON.stringify(err.error.errors), undefined)
          }
        );
    }
  }

  onDelete(): void {
    this.service.deleteStudent(this.student.id)
      .subscribe(
        (successResponse) => {
          this.snack.open('Student deleted successfully', undefined, {
            duration: 2000
          });

          setTimeout(() => {
            this.router.navigateByUrl('students');
          }, 2000);
        },
        (err) => {
          // Log
        }
      );
  }

  onAdd(): void {
    if (this.studentDetailsForm?.form.valid) {
      this.service.addStudent(this.student)
        .subscribe(
          (res) => {
            this.snack.open('Student added successfully', undefined, {
              duration: 2000
            });

            setTimeout(() => {
              this.router.navigateByUrl(`students/${res.id}`);
            }, 2000);

          },
          (err) => {
            this.snack.open( JSON.stringify(err.error.errors), undefined)
          }
        );
    }
  }

  uploadImage(event: any): void {
    if (this.studentId) {
      const file: File = event.target.files[0];
      this.service.uploadImage(this.student.id, file)
        .subscribe(
          (res) => {
            this.student.profileImageUrl = res;
            this.setImage();

            // Show a notification
            this.snack.open('Profile Image Updated', undefined, {
              duration: 2000
            });

          },
          (err) => {

          }
        );

    }

  }

  private setImage(): void {
    if (this.student.profileImageUrl) {
      this.displayProfileImageUrl = this.service.getImagePath(this.student.profileImageUrl);
    } else {
      // Display a default
      this.displayProfileImageUrl = '/assets/user.png';
    }
  }

}
