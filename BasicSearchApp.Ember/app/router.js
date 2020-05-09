import EmberRouter from '@ember/routing/router';
import config from './config/environment';

export default class Router extends EmberRouter {
  location = config.locationType;
  rootURL = config.rootURL;
}

index: Em.Route.extend({
  route: '/',
  redirectsTo: 'search'
}),



Router.map(function() {
  this.route('search', { path: '/' });
});
