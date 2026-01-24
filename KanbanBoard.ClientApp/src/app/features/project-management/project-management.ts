
import { Component, inject, signal } from '@angular/core';
import { Board } from "./boards/board/board";
import { Dashboard } from "../dashboard/dashboard";
import { Tab } from '../../shared/common/tab/tab';
import { TabPaneDirective } from '../../shared/common/tab/tab-pane.directive';
import { Project } from "./project/project";
import { ProjectCard } from 'src/app/shared/common/project-card/project-card';
import { BoardList } from "./boards/board-list/board-list";
import { BoardDto } from '@core/model/project-management.dto';
import { TableColumn } from 'src/app/shared/common/data-table/data-table.model';
import { BoardService } from '@core/api/board.service';

@Component({
  selector: 'app-project-management',
  standalone: true,
  imports: [
    Tab,
    TabPaneDirective,
    ProjectCard,
    BoardList
],
  templateUrl: './project-management.html',
  styleUrl: './project-management.scss',
})
export class ProjectManagement {

  
}
