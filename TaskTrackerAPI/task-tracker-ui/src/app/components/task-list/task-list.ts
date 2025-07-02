import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { TaskService } from '../../services/task';
import { CommonModule } from '@angular/common';
import { ToastrService, ToastrModule } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './task-list.html',
  styleUrl: './task-list.scss',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ToastrModule
  ]
})
export class TaskListComponent implements OnInit {
  tasks: any[] = [];
  submitted = false;
  isLoading = false;
  taskForm: FormGroup;
  editingTaskId: number | null = null;
  currentPage = 1;
  pageSize = 4;
  totalPages = 0;
  totalPagesArray: number[] = [];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private toastr: ToastrService,
    private cdr: ChangeDetectorRef
  ) {
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      dueDate: ['', Validators.required],
      taskPriorityId: [1, Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.taskService.getPagedTasks(this.currentPage, this.pageSize).subscribe({
      next: (data) => {
        this.tasks = [...data.items];
        this.totalPages = Math.ceil(data.totalCount / this.pageSize);
        this.totalPagesArray = Array.from({ length: this.totalPages }, (_, i) => i + 1);
        this.cdr.detectChanges();
      },
      error: (err) => this.showError(err || 'Something went wrong!')
    });
  }

  changePage(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadTasks();
  }


  onSubmit(): void {
    this.submitted = true;
    this.isLoading = true;
    if (this.taskForm.invalid) {
      this.markFormGroupTouched(this.taskForm);
      return;
    }

    const task = this.taskForm.value;
    const request = this.editingTaskId
      ? this.taskService.update(this.editingTaskId, task)
      : this.taskService.create(task);

    request.subscribe({
      next: () => {
        const action = this.editingTaskId ? 'updated' : 'created';
        this.showSuccess(`Task ${action} successfully!`);
        this.taskForm.reset();
        this.editingTaskId = null;
        this.submitted = false;
        this.isLoading = false;
        this.loadTasks();
      },
      error: (err) => {
        this.isLoading = false;
        this.showError(err || 'Something went wrong!');

      }
    });
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if ((control as any).controls) {
        this.markFormGroupTouched(control as FormGroup);
      }
    });
  }

  editTask(task: any): void {
    this.editingTaskId = task.taskId;
    this.taskForm.patchValue({
      title: task.title,
      description: task.description,
      dueDate: this.formatDate(task.dueDate),
      taskPriorityId: task.taskPriorityId ?? 1
    });
  }

  deleteTask(task: any): void {
    const confirmed = window.confirm(`Are you sure you want to delete the task "${task.title}"?`);
    if (!confirmed) return;

    this.taskService.delete(task.taskId).subscribe({
      next: () => {
        this.showSuccess('Task deleted successfully!');
        this.loadTasks();
      },
      error: (err) => this.showError(err || 'Something went wrong!')
    });
  }

  approveTaskWithJQuery(taskId: number): void {
    this.taskService.approveTaskUsingJQuery(
      taskId,
      () => {
        this.showSuccess('Task status updated!');
        this.loadTasks();
      },
      (errorMsg: string) => {
        this.showError(errorMsg);
      }
    );
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  trackByTaskId(index: number, task: any): number {
    return task.taskId;
  }

  private showSuccess(msg: string): void {
    this.toastr.success(msg, 'Success');
  }

  private showError(message: string): void {
    this.toastr.error(message, 'Error');
  }
}
