import { TestBed, inject } from '@angular/core/testing';

import { PlayGameService } from './play-game.service';

describe('PlayGameService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PlayGameService]
    });
  });

  it('should be created', inject([PlayGameService], (service: PlayGameService) => {
    expect(service).toBeTruthy();
  }));
});
