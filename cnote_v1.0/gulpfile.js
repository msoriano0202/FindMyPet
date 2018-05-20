
/*
    @
    @   GULPFILE TASKS
    @
    @   1.  Vendor JS
    @   2.  Main JS
    @   3.  Compile SCSS
    @   4.  Render HTML / SWIG Files
    @   5.  Font Icons
    @   6.  Images
    @   7.  Watch SCSS + JS + HTML / SWIG Files
    @   8.  Server / BrowserSync
    @   9.  Minify CSS, JS Files
    @   10. Start Server
    @
*/


// Requires

var gulp                = require('gulp');
var browserSync    = require('browser-sync');
var reload              = browserSync.reload;
var sass                = require('gulp-sass');
var cleanCSS         = require('gulp-clean-css');
var uglify               = require('gulp-uglify');
var concat             = require('gulp-concat');
var streamify         = require('gulp-streamify');
var rename            = require('gulp-rename');
var swig                = require('gulp-swig');
var plumber           = require('gulp-plumber');
var autoprefixer     = require('gulp-autoprefixer');
var buffer              = require('vinyl-buffer');
var source             = require('vinyl-source-stream');
var merge              = require('merge-stream');

var distPath = './dist';
var folders = [
    'cnote-main',
    'ecommerce'
];



/*
    1.
    Vendor JS
    -------------------------------------------------------------------------------------------------
*/

gulp.task('vendor-js', function () {
    return gulp.src([
        'node_modules/jquery/dist/jquery.js',
        'node_modules/jquery-ui/ui/widget.js',
        'node_modules/jquery-ui/ui/widgets/mouse.js',
        'node_modules/jquery-ui/ui/widgets/slider.js',
        'node_modules/popper.js/dist/umd/popper.js',
        'node_modules/bootstrap/dist/js/bootstrap.js',
        'node_modules/bootstrap-datepicker/dist/js/bootstrap-datepicker.js',
        'node_modules/jquery.appear/jquery.appear.js',
        'node_modules/jquery-countdown/dist/jquery.countdown.js',
        'node_modules/jquery-circle-progress/dist/circle-progress.js',
        'node_modules/scrollreveal/dist/scrollreveal.js',
        'node_modules/masonry-layout/dist/masonry.pkgd.js',
        'node_modules/imagesloaded/imagesloaded.pkgd.js',
        'node_modules/magnific-popup/dist/jquery.magnific-popup.js',
        'node_modules/owl.carousel/dist/owl.carousel.js',
        'node_modules/owl.carousel2.thumbs/dist/owl.carousel2.thumbs.js',
        'node_modules/typed.js/lib/typed.js',
        'node_modules/countup.js/dist/countUp.js'
    ])
        .pipe(gulp.dest(distPath+'/assets/vendor/js/'));
});


/*
    2.
    Main JS FILES
    -------------------------------------------------------------------------------------------------
*/

gulp.task('main-js', function() {
    
    var files = folders.map(function (element) {
        return gulp.src('src/'+element+ '/assets/js/*.js')
            .pipe(plumber())
            .pipe(gulp.dest(distPath+'/'+element +'/assets/js/'));
    })
    return merge(files);
    
});

/*
    3.
    Compile SCSS
    -------------------------------------------------------------------------------------------------
*/

gulp.task('main-scss', function() {
    var files = folders.map(function (element) {
        return gulp.src('src/'+element+ '/assets/scss/**/*.scss')
            .pipe(sass().on('error', sass.logError ))
            .pipe(autoprefixer('last 2 versions'))
            .pipe(gulp.dest(distPath+'/'+element +'/assets/css/'))
            .pipe(reload({ stream: true }));
    })
    return merge(files);
});

gulp.task('vendor-scss', function () {
    return gulp.src("src/assets/vendor/scss/*.scss")
        .pipe(sass().on('error', sass.logError ))
        .pipe(autoprefixer('last 2 versions'))
        .pipe(gulp.dest(distPath+'/assets/vendor/css/'))
        .pipe(reload({ stream: true }));
});



/*
    4.
    Render HTML / SWIG Files
    -------------------------------------------------------------------------------------------------
*/


gulp.task('html-cnotemain', function() {
    gulp.src(['src/cnote-main/views/**/*.html',  '!src/cnote-main/views/_partials/**/*.html'])
        .pipe(swig({defaults: { cache: false }}))
        .on('error', function (err) {
            console.log(err.toString());
            this.emit('end');
        })
        .pipe(gulp.dest(distPath+'/cnote-main/'));
    
    gulp.src('src/*.html')
        .pipe(swig({defaults: {cache:false}}))
        .on('error', function (err) {
            console.log(err.toString());
            this.emit('end');
        })
        .pipe(gulp.dest(distPath))
});

gulp.task('html-ecommerce', function() {
    gulp.src(['src/ecommerce/views/*.html',  '!src/ecommerce/views/_partials/**/*.html'])
        .pipe(swig({defaults: { cache: false }}))
        .on('error', function (err) {
            console.log(err.toString());
            this.emit('end');
        })
        .pipe(gulp.dest(distPath+'/ecommerce/'))
});

gulp.task('html', ['html-cnotemain', 'html-ecommerce']);


/*
    5.
    Font Icons
    -------------------------------------------------------------------------------------------------
*/

gulp.task('fonts', function() {
    return gulp.src([
        'node_modules/font-awesome/fonts/fontawesome-webfont.*',
        'node_modules/simple-line-icons/fonts/Simple-Line-Icons.*',
    ])
        .pipe(gulp.dest(distPath+'/assets/fonts/'));
});


/*
    6.
    Images ( copy from src to dist )
    -------------------------------------------------------------------------------------------------
*/

gulp.task('images', function() {
    return gulp.src('src/assets/images/**/*')
        .pipe(gulp.dest(distPath+'/assets/images/'));
});



/*
    7.
    Watch SCSS + JS + HTML/SWIG Files
    -------------------------------------------------------------------------------------------------
*/

gulp.task('watch', function () {
    gulp.watch("src/**/*.scss", ['main-scss']);
    gulp.watch("src/**/*.js", ['main-js']);
    gulp.watch("src/**/*.html", ['html']);
    gulp.watch(distPath+'/**/*.html').on('change', reload);

});

/*
    8.
    Server / BrowserSync
    -------------------------------------------------------------------------------------------------
*/

gulp.task('server', function() {
    browserSync({
        server: distPath
    });
});


/*
    9.
    Minify CSS, JS Files from dist
    -------------------------------------------------------------------------------------------------
*/

gulp.task('cssminify', function() {
    return gulp.src([distPath+'/**/*.css', '!'+distPath+'/**/*.min.css'])
        .pipe(cleanCSS({compatibility: 'ie8'}))
        .pipe(rename({ extname: '.min.css'}))
        .pipe(gulp.dest(distPath));
});

gulp.task('jsminify', function() {
    return gulp.src([distPath+'/**/*.js', '!'+distPath+'/**/*.min.js'])
        .pipe(uglify())
        .pipe(rename({
            extname: '.min.js'
        }))
        .pipe(gulp.dest(distPath));
});

// All Together
gulp.task('minify', ['cssminify', 'jsminify']);



/*
    10.
    START SERVER
    -------------------------------------------------------------------------------------------------
    "start" task to start the server on localhost
*/

gulp.task('start', ['main-scss', 'main-js', 'vendor-scss', 'vendor-js', 'html', 'fonts', 'server', 'watch']);
