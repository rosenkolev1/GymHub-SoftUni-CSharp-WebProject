
mapboxgl.accessToken = 'pk.eyJ1Ijoicm9zZW5hbmRyZWV2a29sZXYiLCJhIjoiY2tpa2locmJ1MGE5czJ4cWpzdmFocHpmeiJ9.QWnhCblP1_ruT5vC_M61Vg';
var map = new mapboxgl.Map({
    container: 'map',
    style: 'mapbox://styles/mapbox/streets-v11'
});

// Add zoom and rotation controls to the map.
map.addControl(new mapboxgl.NavigationControl());