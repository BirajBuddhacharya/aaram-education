<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AaramEducation.Web.DefaultPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<!-- ══ Hero ════════════════════════════════════════════════════════════════ -->
<section class="container" style="padding-top:72px;padding-bottom:88px">
  <div class="row align-items-center gy-5">
    <div class="col-lg-6">
      <h1 class="t-hero mb-0">Learn in the<br />time you have.</h1>
      <p class="t-lead mt-4 mb-4" style="max-width:30em">
        Short lessons in maths, science, English and computing, with notes to keep,
        quizzes that explain your mistakes, and a tutor for when you're stuck.
      </p>
      <div class="d-flex gap-3 flex-wrap mb-3">
        <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="btn-aaram btn-aaram-primary">Try a free lesson</a>
        <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="btn-aaram btn-aaram-outline">See all courses</a>
      </div>
      <p class="t-xs mb-0">No timetable and no tutor fees. Free samples open without an account.</p>
    </div>

    <div class="col-lg-6">
      <div class="time-card">
        <div class="time-head">
          <span class="time-head-word">I have</span>
          <div class="time-picker" role="group" aria-label="Minutes available">
            <button type="button" class="time-pill" data-mins="5">5</button>
            <button type="button" class="time-pill active" data-mins="10">10</button>
            <button type="button" class="time-pill" data-mins="15">15</button>
          </div>
          <span class="time-head-word">minutes</span>
        </div>

        <div id="fitList">
          <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="lesson-row" data-mins="15">
            <span class="subject-dot subject-maths"></span>
            <span>
              <span class="lesson-row-title d-block">Word problems</span>
              <span class="lesson-row-sub d-block">Algebra Foundations</span>
            </span>
            <span class="lesson-row-time">15 min</span>
          </a>
          <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="lesson-row" data-mins="14">
            <span class="subject-dot subject-computing"></span>
            <span>
              <span class="lesson-row-title d-block">Web page layout</span>
              <span class="lesson-row-sub d-block">Web Basics: HTML and CSS</span>
            </span>
            <span class="lesson-row-time">14 min</span>
          </a>
          <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="lesson-row" data-mins="10">
            <span class="subject-dot subject-computing"></span>
            <span>
              <span class="lesson-row-title d-block">Loops in Python</span>
              <span class="lesson-row-sub d-block">Programming with Python</span>
            </span>
            <span class="lesson-row-time">10 min</span>
          </a>
          <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="lesson-row" data-mins="9">
            <span class="subject-dot subject-english"></span>
            <span>
              <span class="lesson-row-title d-block">A strong opening line</span>
              <span class="lesson-row-sub d-block">Essay Writing</span>
            </span>
            <span class="lesson-row-time">9 min</span>
          </a>
          <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="lesson-row" data-mins="8">
            <span class="subject-dot subject-maths"></span>
            <span>
              <span class="lesson-row-title d-block">Solving linear equations</span>
              <span class="lesson-row-sub d-block">Algebra Foundations</span>
            </span>
            <span class="lesson-row-time">8 min</span>
          </a>
          <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="lesson-row" data-mins="7">
            <span class="subject-dot subject-science"></span>
            <span>
              <span class="lesson-row-title d-block">Speed or velocity?</span>
              <span class="lesson-row-sub d-block">Motion and Forces</span>
            </span>
            <span class="lesson-row-time">7 min</span>
          </a>
          <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="lesson-row" data-mins="5">
            <span class="subject-dot subject-maths"></span>
            <span>
              <span class="lesson-row-title d-block">One-step equations</span>
              <span class="lesson-row-sub d-block">Algebra Foundations</span>
            </span>
            <span class="lesson-row-time">5 min</span>
          </a>
          <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="lesson-row" data-mins="4">
            <span class="subject-dot subject-english"></span>
            <span>
              <span class="lesson-row-title d-block">Nouns and verbs</span>
              <span class="lesson-row-sub d-block">Grammar Essentials</span>
            </span>
            <span class="lesson-row-time">4 min</span>
          </a>
        </div>

        <p class="t-xs mt-3 mb-0" id="fitNote">Enough for a full lesson and a look at the notes.</p>
      </div>
    </div>
  </div>
</section>

<!-- ══ Four ways in ════════════════════════════════════════════════════════ -->
<section class="section-white section-py">
  <div class="container">
    <h2 class="t-h2 mb-5" style="max-width:15em">
      Every topic, four ways in.<br />Use as many as you need.
    </h2>

    <div class="row g-4 g-lg-5">
      <div class="col-sm-6 col-lg-3">
        <div class="step-col">
          <div class="step-num">1</div>
          <div class="t-h4 mb-2">Watch</div>
          <p class="t-small mb-0">A 5 to 10 minute video explains the idea. Pause it, rewind it, or come back tomorrow.</p>
        </div>
      </div>
      <div class="col-sm-6 col-lg-3">
        <div class="step-col">
          <div class="step-num">2</div>
          <div class="t-h4 mb-2">Read</div>
          <p class="t-small mb-0">A one-page summary of the key points. Print it or keep it for revision offline.</p>
        </div>
      </div>
      <div class="col-sm-6 col-lg-3">
        <div class="step-col">
          <div class="step-num">3</div>
          <div class="t-h4 mb-2">Check</div>
          <p class="t-small mb-0">A few practice questions with instant feedback that explains every answer.</p>
        </div>
      </div>
      <div class="col-sm-6 col-lg-3">
        <div class="step-col step-accent">
          <div class="step-num">4</div>
          <div class="t-h4 mb-2">Ask</div>
          <p class="t-small mb-0">Still stuck? Book a one-to-one session with a tutor at a time that suits you.</p>
        </div>
      </div>
    </div>
  </div>
</section>

<!-- ══ Subjects ════════════════════════════════════════════════════════════ -->
<section class="section-py">
  <div class="container">
    <div class="row gy-4">
      <div class="col-lg-4">
        <h2 class="t-h2 mb-3">Subjects</h2>
        <p class="t-small mb-4" style="max-width:22em">
          The four subjects students ask for help with most. More are on the way.
        </p>
        <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="link-aaram">Browse all courses &rarr;</a>
      </div>

      <div class="col-lg-7 offset-lg-1">
        <a href='<%= ResolveUrl("~/Courses/Index.aspx?subject=Mathematics") %>' class="subject-link">
          <span class="subject-bar subject-maths"></span>
          <span>
            <span class="t-h3 d-block">Mathematics</span>
            <span class="t-xs d-block mt-1">Algebra, trigonometry, geometry</span>
          </span>
          <span class="t-xs">16 lessons</span>
        </a>
        <a href='<%= ResolveUrl("~/Courses/Index.aspx?subject=Science") %>' class="subject-link">
          <span class="subject-bar subject-science"></span>
          <span>
            <span class="t-h3 d-block">Science</span>
            <span class="t-xs d-block mt-1">Physics, chemistry, biology</span>
          </span>
          <span class="t-xs">18 lessons</span>
        </a>
        <a href='<%= ResolveUrl("~/Courses/Index.aspx?subject=English") %>' class="subject-link">
          <span class="subject-bar subject-english"></span>
          <span>
            <span class="t-h3 d-block">English</span>
            <span class="t-xs d-block mt-1">Writing, grammar, comprehension</span>
          </span>
          <span class="t-xs">14 lessons</span>
        </a>
        <a href='<%= ResolveUrl("~/Courses/Index.aspx?subject=Computing") %>' class="subject-link">
          <span class="subject-bar subject-computing"></span>
          <span>
            <span class="t-h3 d-block">Computing</span>
            <span class="t-xs d-block mt-1">Programming, web, data</span>
          </span>
          <span class="t-xs">21 lessons</span>
        </a>
      </div>
    </div>
  </div>
</section>

<!-- ══ Audience ════════════════════════════════════════════════════════════ -->
<section class="section-tint section-py">
  <div class="container">
    <div class="row g-5">
      <div class="col-md-4">
        <div class="t-h4 mb-3">For students</div>
        <p class="t-body mb-3" style="max-width:26em">
          Cover what you missed, revise at your own pace, and take practice quizzes
          without any pressure. Pick up exactly where you left off.
        </p>
        <a href='<%= ResolveUrl("~/Account/Register.aspx") %>' class="link-aaram">Create a free account &rarr;</a>
      </div>
      <div class="col-md-4">
        <div class="t-h4 mb-3">For tutors</div>
        <p class="t-body mb-3" style="max-width:26em">
          Upload lessons and notes, build quizzes, and track your students' progress
          from one simple dashboard. No technical knowledge needed.
        </p>
        <a href='<%= ResolveUrl("~/Account/Register.aspx") %>' class="link-aaram">Join as a tutor &rarr;</a>
      </div>
      <div class="col-md-4">
        <div class="t-h4 mb-3">Just browsing</div>
        <p class="t-body mb-3" style="max-width:26em">
          Free sample lessons are open without an account. Have a look around and
          see if it is a good fit before signing up.
        </p>
        <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="link-aaram">Browse free lessons &rarr;</a>
      </div>
    </div>
  </div>
</section>

<!-- ══ Quote ═══════════════════════════════════════════════════════════════ -->
<section class="container section-py">
  <blockquote class="quote-text">
    &ldquo;I came back to maths after fifteen years to help my daughter. Nobody rushed me.&rdquo;
  </blockquote>
  <div class="d-flex align-items-center gap-3 flex-wrap mt-4">
    <span class="t-small mb-0">Ramesh T., from the guest book</span>
    <a href='<%= ResolveUrl("~/Guestbook/Index.aspx") %>' class="link-aaram">Read more messages</a>
  </div>
</section>

<!-- ══ CTA ═════════════════════════════════════════════════════════════════ -->
<section class="container" style="padding-bottom:96px">
  <div class="cta-banner">
    <h2>Five minutes is enough to start.</h2>
    <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="btn-aaram btn-aaram-gold">Try a free lesson</a>
  </div>
</section>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsContent" runat="server">
<script>
  (function () {
    const notes = {
      5:  'Short enough to squeeze in before the bus. No account needed.',
      10: 'Enough for a full lesson and a look at the notes.',
      15: 'Time for a lesson, the notes and a practice quiz.'
    };
    const rows = Array.from(document.querySelectorAll('#fitList .lesson-row'));
    const note = document.getElementById('fitNote');

    function apply(max) {
      let shown = 0;
      rows.forEach(r => {
        const fits = Number(r.dataset.mins) <= max && shown < 4;
        r.style.display = fits ? '' : 'none';
        if (fits) shown++;
      });
      note.textContent = notes[max];
    }

    document.querySelectorAll('.time-pill').forEach(pill => {
      pill.addEventListener('click', function () {
        document.querySelectorAll('.time-pill').forEach(p => p.classList.remove('active'));
        this.classList.add('active');
        apply(Number(this.dataset.mins));
      });
    });

    apply(10);
  })();
</script>
</asp:Content>
