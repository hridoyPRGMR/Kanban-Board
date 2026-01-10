export interface NavItem {
  path: string;
  title: string;
  icon?: string;
  exact?: boolean;
}

export const NAV_ITEMS: NavItem[] = [
  {
    path: '/',
    title: 'Dashboard',
    icon: 'board',
    exact: true,
  },
  {
    path: '/project',
    title: 'Project Management',
    icon: 'project',
  },
  {
    path: '/settings',
    title: 'Settings',
    icon: 'settings',
  },
];
