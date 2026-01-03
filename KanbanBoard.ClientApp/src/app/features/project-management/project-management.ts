import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { Board } from "./board/board";
import { Dashboard } from "../dashboard/dashboard";
import { Tab } from '../../shared/common/tab/tab';
import { TabPaneDirective } from '../../shared/common/tab/tab-pane.directive';
import { Project } from "./project/project";

@Component({
  selector: 'app-project-management',
  standalone: true,
  imports: [CommonModule, Tab, TabPaneDirective, Board, Dashboard, Project],
  templateUrl: './project-management.html',
  styleUrl: './project-management.scss',
})
export class ProjectManagement {

  
}
