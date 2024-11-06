from rester import component, html, run

@component
def hello_world():
    return html.h1("Hello, World!")

run(hello_world)