
import { Component, effect, inject, OnInit, signal } from '@angular/core';
import { Board } from "./boards/board/board";
import { Dashboard } from "../dashboard/dashboard";
import { Tab } from '../../shared/common/tab/tab';
import { TabPaneDirective } from '../../shared/common/tab/tab-pane.directive';
import { Project } from "./project/project";
import { ProjectCard } from 'src/app/shared/common/project-card/project-card';
import { BoardList } from "./boards/board-list/board-list";
import { BoardDto, ProjectDto } from '@core/model/project-management.dto';
import { TableColumn } from 'src/app/shared/common/data-table/data-table.model';
import { BoardService } from '@core/api/board.service';
import { SlideOver } from "src/app/shared/common/slide-over/slide-over";
import { ProjectForm } from "./project/project-form/project-form";
import { ProjectService } from '@core/api/project.service';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { PaginatedResponse } from '@core/model/common.dto';
import { combineLatest, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';

@Component({
  selector: 'app-project-management',
  standalone: true,
  imports: [
    Tab,
    TabPaneDirective,
    ProjectCard,
    BoardList,
    SlideOver,
    ProjectForm
  ],
  templateUrl: './project-management.html',
  styleUrl: './project-management.scss',
})
export class ProjectManagement implements OnInit {

  protected isSlideOverOpen = signal(false);
  private readonly projectService = inject(ProjectService);
  protected projectSearchTerm = signal('');
  protected currentPage = signal(1);
  protected pageSize = signal(5);
  protected isDescending = signal(true);

  projectsResponse = toSignal(
    combineLatest([
      toObservable(this.currentPage),
      toObservable(this.projectSearchTerm).pipe(
        debounceTime(500),
        distinctUntilChanged()
      ),
      toObservable(this.isDescending),
    ]).pipe(
      switchMap(([page, term,isDesc]) =>
        this.projectService.getProjects(term, page, this.pageSize(),isDesc)
      )
    ),
    {
      initialValue: {
        items: [], totalCount: 0, pageNumber: 1,
        pageSize: 10, totalPages: 0, hasNextPage: false, hasPreviousPage: false
      }
    }
  );

  constructor() {
    effect(() => {
      this.projectSearchTerm();
      this.currentPage.set(1);
    });
  }

  ngOnInit(): void {

  }

  toggleSlideOver() {
    this.isSlideOverOpen.update(val => !val);
  }

  onPageInputChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const newPage = Number(input.value);
    const maxPages = this.projectsResponse().totalPages;

    if (newPage >= 1 && newPage <= maxPages) {
      this.goToPage(newPage);
    } else {
      // Reset input to current valid page if they type something crazy
      input.value = this.currentPage().toString();
    }
  }

  goToPage(page: number) {
    const maxPages = this.projectsResponse().totalPages;
    if (page >= 1 && page <= maxPages) {
      this.currentPage.set(page);
    }
  }

}
