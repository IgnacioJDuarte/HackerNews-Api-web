import { HttpClient } from '@angular/common/http';
import { Component, Inject, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-hacker-news-stories',
  templateUrl: './hacker-news-stories.component.html',
  styleUrls: ['./hacker-news-stories.component.css']
})
export class HackerNewsStoriesComponent {
  public stories: HackerNewStories[] = [];
  public currentPageData: HackerNewStories[] = [];
  public columns: string[] = ['Title', 'Url'];
  public dataSource: any;

  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<HackerNewStories[]>(baseUrl + 'hackernews').subscribe((result: HackerNewStories[]) => {
      this.stories = result;
      this.dataSource = new MatTableDataSource<HackerNewStories>(this.stories);
      this.dataSource.paginator = this.paginator;
    }, (error: unknown) => console.error(error));    
  }

  public goToStoryUrl(url: string) {
    window.location.href = url;
  }
}

interface HackerNewStories {
  title: string;
  url: string;
}
